using Consolidado.Application.Interfaces;
using Consolidado.Application.Repositories;
using Consolidado.Application.Events;
using Microsoft.Extensions.Logging;

namespace Consolidado.Application.Handlers
{
    public class TransactionEventHandler : ITransactionEventHandler
    {
        private readonly IConsolidationRepository _consolidationRepository;
        private readonly ILogger<TransactionEventHandler> _logger;

        public TransactionEventHandler(IConsolidationRepository consolidationRepository, ILogger<TransactionEventHandler> logger)
        {
            _consolidationRepository = consolidationRepository;
            _logger = logger;
        }

        public async Task HandleTransactionEvent(TransactionEvent transactionEvent)
        {
            _logger.LogInformation("Recebendo evento de transação: Amount={Amount}, Type={Type}", transactionEvent.Amount, transactionEvent.Type);
            await _consolidationRepository.UpdateBalanceAsync(transactionEvent);
            _logger.LogInformation("Transação atualizada com sucesso: Amount={Amount}, Type={Type}", transactionEvent.Amount, transactionEvent.Type);
        }
    }
}
