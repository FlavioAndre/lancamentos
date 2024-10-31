namespace Consolidado.Application.Events
{
    public class TransactionEvent
    {
        public decimal Amount { get; set; }
        public string Type { get; set; } 
        public DateTime CreatedAt { get; set; }

        public TransactionEvent(decimal amount, string type, DateTime createdAt)
        {
            Amount = amount;
            Type = type;
            CreatedAt = createdAt;
        }
    }
}
