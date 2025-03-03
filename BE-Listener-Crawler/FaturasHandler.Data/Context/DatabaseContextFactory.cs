using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FaturasHandler.Data.Context
{
    public class FaturaDbContextFactory : IDesignTimeDbContextFactory<FaturaDbContext>
    {
        public FaturaDbContext CreateDbContext(string[] args)
        {
            var connectionString = "Data Source=VELLOSO-YOGA\\SQLEXPRESS;Initial Catalog=fatura;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True;Connect Timeout=60;";

            var optionsBuilder = new DbContextOptionsBuilder<FaturaDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new FaturaDbContext(optionsBuilder.Options); // ✅ Uses the new constructor
        }
    }
}
