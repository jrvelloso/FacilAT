using System.Collections.Concurrent;
using FaturasHandler.Crawler;
using FaturasHandler.Crawler.RecibosVerdes;
using FaturasHandler.Crawler.Startup;
using FaturasHandler.IoC.Dto;
using FaturasHandler.IoC.Mapper;
using FaturasHandler.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FaturasHandler.Listener.Services
{
    public class UserActionListener : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<UserActionListener> _logger;
        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(10); // Adjust frequency as needed

        public UserActionListener(IServiceScopeFactory serviceScopeFactory, ILogger<UserActionListener> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("UserActionListener started.");

            var activeTasks = new ConcurrentDictionary<Guid, Task>();
            var semaphore = new SemaphoreSlim(1); // ✅ Limit concurrent actions

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var userActionService = scope.ServiceProvider.GetRequiredService<IUserActionService>();
                        var actions = await userActionService.GetAllPendingActionsAsync(stoppingToken);

                        if (actions.Any())
                        {
                            foreach (var action in actions)
                            {
                                if (activeTasks.ContainsKey(action.UserId))
                                {
                                    _logger.LogWarning($"Skipping duplicate processing for User {action.UserId}");
                                    continue; // ✅ Skip if already being processed
                                }

                                await semaphore.WaitAsync(stoppingToken); // ✅ Concurrency limit

                                activeTasks[action.UserId] = Task.Run(async () =>
                                {
                                    try
                                    {
                                        _logger.LogInformation($"Processing UserAction ID: {action.Id} for User {action.UserId}");

                                        using (var taskScope = _serviceScopeFactory.CreateScope()) // ✅ Create a new scope inside Task
                                        {
                                            var scopedUserActionService = taskScope.ServiceProvider.GetRequiredService<IUserActionService>();

                                            using (var timeoutCts = new CancellationTokenSource(TimeSpan.FromMinutes(5))) // ✅ Prevent hanging indefinitely
                                            using (var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, timeoutCts.Token))
                                            {
                                                // ✅ Start the crawler with timeout
                                                await StartCrawler(action.UserId, action.User.NIF, action.User.Password, linkedCts.Token);

                                                action.Active = false;
                                                await scopedUserActionService.UpdateAsync(action, stoppingToken); // ✅ Use scoped service
                                            }
                                        }
                                    }
                                    catch (OperationCanceledException)
                                    {
                                        _logger.LogError($"Processing timeout for UserAction {action.Id}. Task was cancelled.");
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError($"[ERROR] Processing UserAction ID {action.Id}: {ex.Message}");
                                    }
                                    finally
                                    {
                                        activeTasks.TryRemove(action.UserId, out _);
                                        semaphore.Release(); // ✅ Always release semaphore
                                    }
                                }, stoppingToken);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"[ERROR] UserActionListener: {ex.Message}");
                }

                await Task.Delay(_checkInterval, stoppingToken); // ✅ Ensure pause between iterations
            }

            _logger.LogInformation("UserActionListener stopped.");
        }

        private async Task StartCrawler(Guid userId, string nif, string password, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Starting Crawler for User ID: {userId}...");

                var browserAndPage = await AT.ConnectToAT(nif, password); // Ensure login happens before API call

                // ✅ Fetch Recibos Verdes and IVA Declarations with cancellation support
                Thread.Sleep(2000);
                List<RecibosVerdesDto> listRecibosVerdeDto = await ListagemRecibosVerdes.FetchDataListagemRecibosVerdes(nif, password, browserAndPage.page, cancellationToken);
                Thread.Sleep(2000);
                List<ListagemDeclaracaoEntreguesDto> listTax = await ListagemDeclaracoesEntregues.FetchDataListagemDeclaracoesEntregues(nif, password, browserAndPage.page, cancellationToken);
                Thread.Sleep(2000);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var revService = scope.ServiceProvider.GetRequiredService<IReciboVerdeService>();
                    var ivaService = scope.ServiceProvider.GetRequiredService<IIVADeclarationService>();

                    // ✅ Ensure that AddManyAsync supports cancellation
                    var invoicesCount = await revService.AddManyAsync(RecibosVerdesMapper.ToManyEntities(listRecibosVerdeDto, userId), userId, cancellationToken);
                    var ivaCount = await ivaService.AddManyAsync(IVADeclarationMapper.ToManyEntities(listTax, userId), userId, cancellationToken);
                }

                _logger.LogInformation($"Crawler completed successfully for User ID: {userId}.");

                if (browserAndPage.browser != null)
                    await browserAndPage.browser.CloseAsync();

                _logger.LogInformation($"Browser closed {userId}.");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning($"Crawler operation was canceled for User ID: {userId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to complete Crawler for User ID: {userId}: {ex.Message}");
            }
        }

    }
}
