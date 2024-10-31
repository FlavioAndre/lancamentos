using ControleLancamentos.Domain.Shared;

namespace ControleLancamentos.Domain.ValueObjects
{
    public class TransactionType : ValueObject
    {
        public static readonly TransactionType Credit = new("Credit");
        public static readonly TransactionType Debit = new("Debit");

        public string Value { get; }

        private TransactionType(string value)
        {
            Value = value.ToLowerInvariant();
        }

        public static TransactionType FromString(string value)
        {
            return value.ToLower() switch
            {
                "credit" => Credit,
                "debit" => Debit,
                _ => throw new ArgumentException("Tipo inválido de transação!")
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}