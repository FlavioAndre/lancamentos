using ControleLancamentos.Application.Commands;
using ControleLancamentos.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;

namespace ControleLancamentos.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ILogger<TransactionService> _logger;
        private readonly AsyncPolicy _retryPolicy;

        public TransactionService(
            ITransactionRepository transactionRepository,
            IMessagePublisher messagePublisher,
            ILogger<TransactionService> logger)
        {
            _transactionRepository = transactionRepository;
            _messagePublisher = messagePublisher;
            _logger = logger;

            _retryPolicy = Policy.Handle<Exception>()
                                 .CircuitBreakerAsync(2, TimeSpan.FromMinutes(1),
                                    onBreak: (ex, breakDelay) =>
                                    {
                                        _logger.LogError("Circuit Breaker ativado por {BreakDelay}", breakDelay);
                                    },
                                    onReset: () => _logger.LogInformation("Circuit Breaker resetado"),
                                    onHalfOpen: () => _logger.LogInformation("Circuit Breaker em Half-Open"))
                                 .WrapAsync(Policy.Handle<Exception>()
                                                  .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                                                  onRetry: (exception, retryCount) =>
                                                  {
                                                      _logger.LogWarning("Tentativa {RetryCount}", retryCount);
                                                  }));
        }

        public async Task AddTransactionAsync(CreateTransactionCommand transaction)
        {
            await _transactionRepository.AddAsync(transaction);
            await _retryPolicy.ExecuteAsync(async () =>
            {
                await _messagePublisher.PublishAsync("transactionQueue", transaction);
            });
        }
    }
}
