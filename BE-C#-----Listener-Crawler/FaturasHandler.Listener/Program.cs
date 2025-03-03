using FaturasHandler.Data.Context;
using FaturasHandler.Data.Repository;
using FaturasHandler.IoC;
using FaturasHandler.Listener.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var basePath = AppContext.BaseDirectory; // ✅ Ensures correct path
        config.SetBasePath(basePath);
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        var connectionString = context.Configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<FaturaDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IUserActionRepository, UserActionRepository>();
        services.AddHostedService<UserActionListener>();
        services.AddInfrastructure();

    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
    })
    .Build();

await host.RunAsync();
