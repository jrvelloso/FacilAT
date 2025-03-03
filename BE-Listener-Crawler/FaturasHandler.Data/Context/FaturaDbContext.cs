using FaturasHandler.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FaturasHandler.Data.Context
{
    public class FaturaDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public FaturaDbContext(DbContextOptions<FaturaDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }


        public FaturaDbContext(DbContextOptions<FaturaDbContext> options) : base(options) { }

        public DbSet<UserData> Users { get; set; }
        public DbSet<IVADeclaration> TaxDeclarations { get; set; }
        public DbSet<ReciboVerde> Invoices { get; set; }
        public DbSet<UserAction> UserActions { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserData>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserData>()
                .HasIndex(u => u.NIF)
                .IsUnique();
        }
    }
}
