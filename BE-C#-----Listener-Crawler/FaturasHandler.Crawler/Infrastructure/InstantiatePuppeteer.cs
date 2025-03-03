using System.Diagnostics;
using Newtonsoft.Json;
using PuppeteerSharp;

namespace FaturasHandler.Crawler.Infrastructure
{
    public static class InstantiatePuppeteer
    {
        private static void KillEdge()
        {
            foreach (var process in Process.GetProcessesByName("msedge"))
            {
                process.Kill();
            }
        }

        public static async Task<IPage> InstantiatePageAsync(Browser browser, string url, CancellationToken cancellationToken)
        {
            IPage page = await browser.NewPageAsync();
            await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            await page.SetViewportAsync(new ViewPortOptions
            {
                Width = 1920,
                Height = 1080
            });
            await page.GoToAsync(url); // Added cancellation token
            return page;
        }

        public static async Task<Browser> InstantiateBrowserAsync(CancellationToken cancellationToken, bool isHeadless = false)
        {
            try
            {
                //KillEdge();
                IBrowser browser;
                string webSocketDebuggerUrl = await GetWebSocketDebuggerUrlAsync();

                if (webSocketDebuggerUrl != null && await IsBrowserRunning(webSocketDebuggerUrl))
                {
                    Console.WriteLine("Connecting to existing browser instance...");
                    browser = await Puppeteer.ConnectAsync(new ConnectOptions
                    {
                        BrowserWSEndpoint = webSocketDebuggerUrl,
                    });
                    Console.WriteLine("Connected to existing browser.");
                }
                else
                {
                    Console.WriteLine("Launching a new browser instance...");
                    browser = await Puppeteer.LaunchAsync(new LaunchOptions
                    {
                        Headless = isHeadless,
                        Args = new[] {
                            "--no-sandbox",
                            "--disable-setuid-sandbox",
                            "--disable-dev-shm-usage",
                            "--remote-debugging-port=9222",
                            "--profile-directory=Default"
                        },
                        ExecutablePath = "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe"
                    });
                    Console.WriteLine("Launched a new browser instance.");
                }

                return (Browser)browser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private static async Task<string> GetWebSocketDebuggerUrlAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetStringAsync("http://localhost:9222/json/version");
                    dynamic jsonResponse = JsonConvert.DeserializeObject(response);
                    return jsonResponse.webSocketDebuggerUrl;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving WebSocket URL: {ex.Message}");
                return null;
            }
        }

        private static async Task<bool> IsBrowserRunning(string webSocketDebuggerUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync($"{webSocketDebuggerUrl}/json/version");
                    return response.IsSuccessStatusCode;
                }
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
