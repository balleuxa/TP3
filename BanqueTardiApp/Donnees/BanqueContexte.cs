using BanqueTardiApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection.Emit;

namespace BanqueTardiApp.Donnees
{
    public class BanqueContexte : DbContext
    {
        public BanqueContexte(DbContextOptions<BanqueContexte> options)
            : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CompteBancaire> ComptesBancaires { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompteBancaire>()
            .HasOne(c => c.Client)
            .WithMany(c => c.Comptes)
            .HasForeignKey(c => c.ClientCode)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Transaction>()
            .HasOne(t => t.CompteBancaire)
            .WithMany(cb => cb.Transactions)
            .HasForeignKey(t => t.NumeroCompte)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
