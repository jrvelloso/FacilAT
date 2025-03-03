using PuppeteerSharp;

namespace FaturasHandler.Crawler.Infrastructure
{
    public static class HandlePages
    {
        //public static async Task<IPage> Start(string url = "https://sitfiscal.portaldasfinancas.gov.pt/geral/dashboard")
        //{
        //    var browser = await InstantiatePuppeteer.InstantiateBrowserAsync(CancellationToken.None);
        //    var page = await InstantiatePuppeteer.InstantiatePageAsync(browser, url, CancellationToken.None);

        //    await Authenticate(page, Constants.NIF, Constants.ATPassword);

        //    return page;
        //}

        public static async Task Authenticate(IPage page, string username, string password)
        {
            Console.WriteLine("🔍 Checking if login is required...");

            await page.WaitForSelectorAsync("body", new WaitForSelectorOptions { Timeout = 10000 });

            Console.WriteLine("🔍 Checking if login is required...");

            var elements = await page.XPathAsync("/html/body/div[1]/header/div/div/div/div/div[2]/p");
            bool requiresLogin = elements.Length > 0;

            if (requiresLogin)
            {
                Console.WriteLine("🔒 Login required. Proceeding with authentication...");

                var nifTab = "#content-area > div > div > label:nth-child(4)";
                await CheckIfExistsAndClick(page, nifTab);
                await LogUserIn(page, username, password);

                Console.WriteLine("⏳ Waiting for login process to complete...");

                // ✅ FIX: Wait for a stable page load before proceeding
                await page.WaitForNavigationAsync(new NavigationOptions
                {
                    WaitUntil = new[] { WaitUntilNavigation.Load }
                });

                Console.WriteLine("✅ Login successful!");
            }
            else
            {
                Console.WriteLine("✅ Already authenticated. Proceeding to fetch data...");
            }
        }

        private static async Task LogUserIn(IPage page, string username, string password)
        {
            try
            {
                Console.WriteLine("🔑 Typing username...");
                await page.TypeAsync("#username", username);

                Console.WriteLine("🔑 Typing password...");
                await page.TypeAsync("#password-nif", password);

                Console.WriteLine("🔑 Clicking login button...");
                await CheckIfExistsAndClick(page, "#sbmtLogin");

                Console.WriteLine("⏳ Waiting for page transition after login...");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Login failed: {ex.Message}");
            }
        }

        private static async Task CheckIfExistsAndClick(IPage page, string selector)
        {
            try
            {
                await page.WaitForSelectorAsync(selector);
                var element = await page.QuerySelectorAsync(selector);
                if (element != null)
                {
                    await element.ClickAsync();
                    Console.WriteLine($"✅ Clicked element: {selector}");
                }
                else
                {
                    Console.WriteLine($"⚠️ Element not found: {selector}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error clicking {selector}: {ex.Message}");
            }
        }
    }
}
