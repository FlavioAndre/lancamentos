using System.Text.Json.Serialization;
using ControleLancamentos.Domain.ValueObjects;

namespace ControleLancamentos.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public required string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("type")] 
        public required TransactionType Type { get; set; }
    }
}