using Consolidado.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Consolidado.Infrastructure.Data
{
    public class ConsolidationDbContext : DbContext
    {
        public ConsolidationDbContext(DbContextOptions<ConsolidationDbContext> options) : base(options) { }

        public DbSet<ConsolidatedBalance> ConsolidatedBalances { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ConsolidatedBalance>(entity =>
            {
                entity.ToTable("consolidated_balance");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.TotalCredit).HasColumnName("total_credit");
                entity.Property(e => e.TotalDebit).HasColumnName("total_debit");
                entity.Property(e => e.Balance).HasColumnName("balance");
                entity.Property(e => e.LastUpdated).HasColumnName("last_updated");             
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

