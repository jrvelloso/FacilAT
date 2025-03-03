using FaturasHandler.Crawler.IVA_Periodico;
using FaturasHandler.Crawler.Startup;
using FaturasHandler.IoC.Dto;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace FaturasHandler.Crawler.Application
{
    public class IVAPeriodicoService
    {
        /// <summary>
        /// This extracts the execel of the quarter in order to export them
        /// </summary>
        /// <returns></returns>
        public async Task ExtractFromEFatura(string nif, string password)
        {
            try
            {
                var url = "https://www.acesso.gov.pt/jsp/loginRedirectForm.jsp?path=consultarDocumentosAdquirente.action&partID=EFPF";

                var browserAndPage = await AT.ConnectToAT(nif, password); // Ensure login happens before API call


                Console.WriteLine("✅ Browser connected.");

                var exporter = new IVAExporter();

                // ✅ Ensure login before fetching data 
                string startDate = "2024-10-01";
                string endDate = "2024-12-31";

                Console.WriteLine($"📅 Fetching invoices from {startDate} to {endDate}...");

                var documents = await FetchDocuments(browserAndPage.page, startDate, endDate);
                documents = ProcessData(documents);

                Console.WriteLine("💾 Saving files...");
                exporter.SaveAsCsv(documents, "Faturas.csv");
                exporter.SaveAsExcel(documents, "Faturas.xlsx");

                Console.WriteLine($"✅ Files generated: Faturas.csv & Faturas.xlsx");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] {ex.Message}");
            }
        }

        public List<FaturaDto> ProcessData(List<FaturaDto> documents)
        {
            decimal total = 0, totalIva = 0, finalValue = 0;

            foreach (var doc in documents)
            {
                total += doc.ValorTotal;
                totalIva += doc.ValorTotalIva;
                finalValue += doc.ValorTotalIva;
            }

            // Add TOTAL row
            documents.Add(new FaturaDto
            {
                DataEmissaoDocumento = "TOTAL",
                NomeEmitente = "",
                NumeroDocumento = "",
                ValorTotal = total,
                ValorTotalIva = totalIva,
                AmbitoDeAtividade = "",
            });

            return documents;
        }

        public async Task<List<FaturaDto>> FetchDocuments(IPage page, string startDate, string endDate)
        {
            var documents = new List<FaturaDto>();

            await FetchAndAddDocuments(page, documents, 0, "Totalmente", startDate, endDate);
            await FetchAndAddDocuments(page, documents, 2, "Parcialmente", startDate, endDate);

            return documents;
        }

        private async Task FetchAndAddDocuments(IPage page, List<FaturaDto> documents, int filterValue, string ambitoLabel, string startDate, string endDate)
        {
            var url = $"https://faturas.portaldasfinancas.gov.pt/json/obterDocumentosAdquirente.action?dataInicioFilter={startDate}&dataFimFilter={endDate}&ambitoAquisicaoFilter=+&tipoAmbitoAtividadeFilter={filterValue}&_={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}";

            Console.WriteLine($"🔍 Fetching data from: {url}");

            try
            {
                // ✅ FIX: Execute fetch in a stable browser context
                var jsonResponse = await page.EvaluateFunctionAsync<string>(@"
                    async () => {
                        try {
                            const response = await fetch('" + url + @"', {
                                method: 'GET',
                                credentials: 'include'
                            });
                            return await response.text();
                        } catch (error) {
                            console.error('Fetch failed:', error);
                            return '{}';
                        }
                    }");

                var response = JsonConvert.DeserializeObject<FaturaResponseDto>(jsonResponse);
                if (response?.Linhas != null)
                {
                    foreach (var doc in response.Linhas)
                    {
                        doc.AmbitoDeAtividade = ambitoLabel;
                        documents.Add(doc);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Fetching documents failed: {ex.Message}");
            }
        }
    }
}
