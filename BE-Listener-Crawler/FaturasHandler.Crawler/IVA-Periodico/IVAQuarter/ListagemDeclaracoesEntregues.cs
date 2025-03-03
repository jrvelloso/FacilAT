using FaturasHandler.IoC.Dto;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace FaturasHandler.Crawler
{
    public static class ListagemDeclaracoesEntregues
    {
        public static async Task<List<ListagemDeclaracaoEntreguesDto>> FetchDataListagemDeclaracoesEntregues(string nif, string password, IPage page, CancellationToken cancellationToken)
        {
            List<ListagemDeclaracaoEntreguesDto> allDeclaracoes = new List<ListagemDeclaracaoEntreguesDto>();

            int currentYear = DateTime.UtcNow.Year;
            int startYear = currentYear - 5;

            for (int year = currentYear; year >= startYear; year--)
            {
                var url = $"https://iva.portaldasfinancas.gov.pt/dpiva/api/lista-declaracoes?ano={year}&isToc=false&periodo=00A";
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

                    var response = JsonConvert.DeserializeObject<ListagemDeclaracaoEntreguesResponseDto>(jsonResponse);

                    if (response?.Declaracoes != null && response?.Declaracoes.Count > 0)
                    {
                        allDeclaracoes.AddRange(response.Declaracoes.Select(declaracao => new ListagemDeclaracaoEntreguesDto
                        {
                            Tipo = declaracao.Tipo,
                            Situacao = declaracao.Situacao,
                            IndicadorPagamentoReembolso = declaracao.IndicadorPagamentoReembolso,
                            BaseTributavelCentimos = declaracao.BaseTributavelCentimos,
                            ImpostoLiquidadoCentimos = declaracao.ImpostoLiquidadoCentimos,
                            ImpostoDedutivelCentimos = declaracao.ImpostoDedutivelCentimos,
                            Valor1 = declaracao.Valor1,
                            Valor2 = declaracao.Valor2,
                            Periodo = declaracao.Periodo,
                            DataRececao = declaracao.DataRececao,
                            NumeroDeclaracao = declaracao.NumeroDeclaracao
                        }));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[ERROR] Fetching documents for {year} failed: {ex.Message}");
                }
            }
            Thread.Sleep(1000);

            return allDeclaracoes;
        }
    }
}
