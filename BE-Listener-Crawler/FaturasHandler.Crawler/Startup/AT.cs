using FaturasHandler.Crawler.Infrastructure;
using PuppeteerSharp;

namespace FaturasHandler.Crawler.Startup
{
    public static class AT
    {
        public static async Task<(IBrowser browser, IPage page)> ConnectToAT(string nif, string password)
        {
            var url = "https://sitfiscal.portaldasfinancas.gov.pt/geral/dashboard";

            var browser = await InstantiatePuppeteer.InstantiateBrowserAsync(CancellationToken.None);
            var page = await InstantiatePuppeteer.InstantiatePageAsync(browser, url, CancellationToken.None);

            await HandlePages.Authenticate(page, nif, password);

            Console.WriteLine("✅ Successfully logged in.");
            return (browser, page); // Return both objects
        }
    }
}
