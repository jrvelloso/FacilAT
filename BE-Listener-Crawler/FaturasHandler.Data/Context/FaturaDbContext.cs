using FaturasHandler.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FaturasHandler.Data.Context
{
    public class FaturaDbContext : DbContext
    {
        public FaturaDbContext(DbContextOptions<FaturaDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserData> Users { get; set; }
        public DbSet<IVADeclaration> TaxDeclarations { get; set; }
        public DbSet<ReciboVerde> Invoices { get; set; }
        public DbSet<UserAction> UserActions { get; set; }

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
