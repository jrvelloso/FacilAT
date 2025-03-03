using FaturasHandler.Data.Context;
using FaturasHandler.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

class Program
{
    static async Task Main(string[] args)
    {
        // Create a Host for Dependency Injection
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register IoC Dependencies
                services.AddInfrastructure();
            })
            .Build();

        // Resolve the DatabaseContext to apply migrations
        using var scope = host.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<FaturaDbContext>();
        await dbContext.Database.MigrateAsync(); // ✅ Apply pending migrations

        Console.WriteLine("SQL Server Database Initialized.");
    }
}
