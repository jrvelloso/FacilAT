using System.Text.Json;
using FaturasHandler.IoC.Dto;
using PuppeteerSharp;

namespace FaturasHandler.Crawler.RecibosVerdes
{
    public static class ListagemRecibosVerdes
    {

        public static async Task<List<RecibosVerdesDto>> FetchDataListagemRecibosVerdes(string nif, string password, IPage page, CancellationToken cancellationToken)
        {

            Thread.Sleep(2000);
            List<RecibosVerdesDto> allRecibos = new List<RecibosVerdesDto>();

            int currentYear = DateTime.UtcNow.Year;
            int startYear = currentYear - 5;

            for (int year = currentYear; year >= startYear; year--)
            {
                var url = $"https://irs.portaldasfinancas.gov.pt/recibos/api/obtemDocumentosV2?dataEmissaoFim=2025-02-19&dataEmissaoInicio=2024-02-19&isAutoSearchOn=on&modoConsulta=Prestador&nifPrestadorServicos={nif}&offset=0&tableSize=5&tipoPesquisa=1";

                await page.GoToAsync(url);

                Console.WriteLine($"🔍 Fetching data for year {year} from: {url}");
                Thread.Sleep(2000);
                try
                {
                    var jsonResponse = await page.EvaluateFunctionAsync<string>(@$"
                        async () => {{
                            try {{
                                const response = await fetch('{url}', {{
                                    method: 'GET',
                                    credentials: 'include'
                                }});
                                return await response.text();
                            }} catch (error) {{
                                console.error('Fetch failed:', error);
                                return '{{}}';
                            }}
                        }}"
                    );

                    var response = JsonSerializer.Deserialize<RecibosVerdesDto>(jsonResponse);

                    if (response?.ListaDocumentos != null)
                    {
                        allRecibos.Add(response);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Fetching documents for {year} failed: {ex.Message}");
                }
            }


            Console.WriteLine("🛑 Browser closed successfully.");
            Thread.Sleep(2000);
            return allRecibos;
        }
    }
}
