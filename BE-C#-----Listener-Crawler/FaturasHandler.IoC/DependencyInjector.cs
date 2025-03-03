using FaturasHandler.Data.Context;
using FaturasHandler.Data.Repository;
using FaturasHandler.Services.Interfaces;
using FaturasHandler.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FaturasHandler.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IIVADeclarationRepository, IVADeclarationRepository>();
            services.AddScoped<IReciboVerdeRepository, ReciboVerdeRepository>();
            services.AddScoped<IUserActionRepository, UserActionRepository>();


            services.AddScoped<IUserActionService, UserActionService>();
            services.AddScoped<IReciboVerdeService, ReciboVerdeService>();
            services.AddScoped<IIVADeclarationService, IVADeclarationService>();
            services.AddScoped<IUserService, UserService>();


            var connectionString = "Data Source=VELLOSO-YOGA\\SQLEXPRESS;Initial Catalog=fatura;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=60;";

            // ✅ Use SQL Server instead of SQLite
            services.AddDbContext<FaturaDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Register other dependencies (Repositories, Services, etc.)
            return services;
        }
    }
}
