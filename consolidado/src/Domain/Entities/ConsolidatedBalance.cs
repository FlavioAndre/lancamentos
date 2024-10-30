namespace Consolidado.Domain.Entities
{
    public class ConsolidatedBalance
    {
        public int Id { get; set; }
        public decimal TotalCredit { get; set; } = 0.00M;
        public decimal TotalDebit { get; set; } = 0.00M;
        public decimal Balance { get; set; } = 0.00M;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
