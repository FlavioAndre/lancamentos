using Consolidado.Application.Events;

namespace Consolidado.Application.Interfaces
{
    public interface ITransactionEventHandler
    {
        Task HandleTransactionEvent(TransactionEvent transactionEvent);
    }
}
