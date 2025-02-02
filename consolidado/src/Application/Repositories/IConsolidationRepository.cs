using Consolidado.Domain.Entities;
using Consolidado.Application.Events;

namespace Consolidado.Application.Repositories
{
    public interface IConsolidationRepository
    {
        Task UpdateBalanceAsync(TransactionEvent transactionEvent);

        Task<ConsolidatedBalance> GetConsolidatedBalanceAsync();
    }
}
