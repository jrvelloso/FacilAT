using FaturasHandler.Crawler.Infrastructure;
using FaturasHandler.Crawler.Startup;
using PuppeteerSharp;

namespace FaturasHandler.Crawler.RecibosVerdes
{
    public class RecibosVerdesService
    {
        private static readonly HttpClientService _httpClientService = new HttpClientService();
        private static readonly int TableSize = 5;
        private static readonly string SaveFilePath = "recibos.json";

        public static async Task GerarReciboVerde(string nif, string password)
        {
            try
            {
                var browserAndPage = await AT.ConnectToAT(nif, password); // Ensure login happens before API call

                var reciboPageURL = "https://irs.portaldasfinancas.gov.pt/recibos/portal/emitir/emitirfaturaV2/";

                await browserAndPage.page.GoToAsync(reciboPageURL);

                await FillRecibo(browserAndPage.page);

                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async static Task FillRecibo(IPage page)
        {
            var nifTMC = "515115266";
            var currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            Console.WriteLine("Starting FillRecibo method...");

            // Date
            Console.WriteLine("Filling in the date...");
            await page.WaitForSelectorAsync("input[name='dataPrestacao']");
            await page.TypeAsync("input[name='dataPrestacao']", currentDate);

            Console.WriteLine("Selecting receipt type...");
            await page.SelectAsync("select[name='tipoRecibo']", "string:FR");

            // Input NIF (tax number) for Portuguese NIF
            Console.WriteLine("Entering NIF...");
            await page.WaitForSelectorAsync("input[name='nifAdquirente']");
            await page.TypeAsync("input[name='nifAdquirente']", nifTMC);

            Console.WriteLine("Clicking 'Procurar Cliente' button...");
            var btnProcurarCliente = "#main-content > div > div > emitir-app-v2 > emitir-form-v2 > div.code-fixed-header > div.row.margin-top-lg > div > dados-adquirente-v2 > div.panel.panel-primary-alt > div.panel-body > div:nth-child(3) > div > button";
            await CheckIfExistsAndClick(page, btnProcurarCliente);

            Console.WriteLine("Clicking 'Selecionar Cliente' button...");
            var btnSelecionarCliente = "#catalogoClientesModal > consultar-emissao-clientes > div > div > div.modal-body > div:nth-child(2) > div > table > tbody > tr > td.text-center > div > button";
            await CheckIfExistsAndClick(page, btnSelecionarCliente);

            Thread.Sleep(1000);

            Console.WriteLine("Selecting 'Documento emitido a título de' radio button...");
            await page.WaitForSelectorAsync("input[name='titulo'][value='1']");
            await page.ClickAsync("input[name='titulo'][value='1']");

            Console.WriteLine("Clicking 'Adicionar Linha' button...");
            await page.WaitForSelectorAsync("button.btn.btn-link[ng-click='$ctrl.onAdicionarLinha()']");
            await page.ClickAsync("button.btn.btn-link[ng-click='$ctrl.onAdicionarLinha()']");

            Thread.Sleep(1000);

            Console.WriteLine("Clicking 'Procurar' button...");
            await page.WaitForSelectorAsync("button.btn.btn-default.btn-sm.col-md-3.pull-right[ng-click='$ctrl.onPressProcurarBemServico()']");
            await page.ClickAsync("button.btn.btn-default.btn-sm.col-md-3.pull-right[ng-click='$ctrl.onPressProcurarBemServico()']");

            Console.WriteLine("Clicking 'Adicionar Produto' button...");
            await page.WaitForSelectorAsync("#adicionarProdutoButton");
            await page.ClickAsync("#adicionarProdutoButton");

            var days = CalcBusinessDays();
            Console.WriteLine($"Calculated business days: {days}");

            Console.WriteLine("Clearing and setting quantity...");
            await page.WaitForSelectorAsync("input[name='quantidade']");
            await page.FocusAsync("input[name='quantidade']");
            await page.EvaluateFunctionAsync("selector => document.querySelector(selector).value = ''", "input[name='quantidade']");
            await page.TypeAsync("input[name='quantidade']", days.ToString() + "000");

            Thread.Sleep(1000);

            Console.WriteLine("Selecting VAT regime...");
            await page.WaitForSelectorAsync("select[name='idTaxaIVA']");
            await page.SelectAsync("select[name='idTaxaIVA']", "number:1"); // 23% - Taxa Normal - Continente

            Console.WriteLine("Clicking 'Save Service' button...");
            var btnSaveServico = "#adicionarProdutosModal > adicionar-produtos > div > div > div.modal-footer > button.btn.btn-primary.btn-sm.ng-scope";
            await CheckIfExistsAndClick(page, btnSaveServico);

            Console.WriteLine("Selecting IRS incidence regime...");
            await page.WaitForSelectorAsync("select[name='regimeIncidenciaIrs']");
            await page.SelectAsync("select[name='regimeIncidenciaIrs']", "string:08"); // Sobre 100% - art. 101.º, n.ºs 1 e 9, do CIRS

            Console.WriteLine("Selecting IRS retention regime...");
            await page.WaitForSelectorAsync("select[name='regimeRetencaoIrs']");
            await page.SelectAsync("select[name='regimeRetencaoIrs']", "string:10"); // À taxa de 20% - art. 101.º, n.º1, do CIRS

            Console.WriteLine("Taking a screenshot...");
            await page.ScreenshotAsync("form_filled.png");

            Console.WriteLine("FillRecibo method completed successfully.");
        }


        private static int CalcBusinessDays()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            int businessDaysCount = 0;
            int daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDate = new DateTime(year, month, day);
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    businessDaysCount++;
                }
            }
            return businessDaysCount;
            //return businessDaysCount;
        }

        public static async Task<bool> IsLoginPagePresent(IPage page)
        {
            try
            {
                // Check for existence of username input field to determine if on login page
                await page.WaitForSelectorAsync("#username", new WaitForSelectorOptions { Timeout = 5000 });
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        public static async Task Login(IPage page, string username, string password)
        {
            if (!await IsLoginPagePresent(page))
                return;

            try
            {
                // Type username
                await page.TypeAsync("#username", username);

                // Type password
                await page.TypeAsync("#password-nif", password);

                await CheckIfExistsAndClick(page, "#sbmtLogin");

                // Wait for navigation or any other expected event after login
                await page.WaitForNavigationAsync();

                // Check if login is successful
                var loggedIn = await page.QuerySelectorAsync("body") != null; // Adjust selector based on post-login state

                if (loggedIn)
                {
                    Console.WriteLine("Login successful!");
                }
                else
                {
                    Console.WriteLine("Login failed!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static async Task CheckIfExistsAndClick(IPage page, string selector)
        {
            await page.WaitForSelectorAsync(selector);

            var element = await page.QuerySelectorAsync(selector);

            if (element != null)
            {
                await element.ClickAsync();
            }
        }

        public static string ConstructURL(string dataEmissaoFim, string nifPrestadorServicos)
        {
            string baseURL = "https://irs.portaldasfinancas.gov.pt/recibos/portal/emitir/emitirfatura";
            string dataCopia = "2024-09-13";
            string tipoRecibo = "FR";
            string dataEmissaoInicio = "1900-07-01";
            string modoConsulta = "Prestador";
            string isAutoSearchOn = "on";

            string queryParameters = $"?dataCopia={dataCopia}&tipoRecibo={tipoRecibo}";
            string fragmentIdentifier = $"#?dataEmissaoFim={dataEmissaoFim}&dataEmissaoInicio={dataEmissaoInicio}&modoConsulta={modoConsulta}&nifPrestadorServicos={nifPrestadorServicos}&isAutoSearchOn={isAutoSearchOn}";

            return $"{baseURL}{queryParameters}{fragmentIdentifier}";
        }
    }
}


