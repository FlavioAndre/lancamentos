using ControleLancamentos.Domain.Entities;
using ControleLancamentos.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ControleLancamentos.Infrastructure.Data
{
    public class TransactionDbContext : DbContext
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options) : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("transactions");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Amount).HasColumnName("amount");

                 modelBuilder.Entity<Transaction>()
                    .Property(e => e.Type)
                    .HasConversion(
                        v => v.Value.ToLower(),
                        v => TransactionType.FromString(v)
                    )
                    .HasColumnName("type")
                    .HasMaxLength(10);


                entity.Property(e => e.Description)
                    .HasColumnName("description");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

            });

            base.OnModelCreating(modelBuilder);
        }
    }


}
