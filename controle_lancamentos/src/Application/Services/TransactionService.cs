using ControleLancamentos.Application.Commands;
using ControleLancamentos.Application.Interfaces;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

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
            _retryPolicy = CreateRetryPolicy();
        }

        /// <summary>
        /// Executes the provided asynchronous action with a retry policy.
        /// This method ensures that transient failures are retried according to the configured policy.
        /// </summary>
        /// <param name="action">The asynchronous action to be executed with retry logic.</param>
        private async Task ExecuteWithRetryAsync(Func<Task> action)
        {
            await _retryPolicy.ExecuteAsync(action);
        }

        private AsyncRetryPolicy CreateRetryPolicy()
        {
            // Configures a retry policy that retries 3 times with a 2-second delay between retries.
            return Policy.Handle<Exception>()
                         .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2),
                         onRetry: (exception, retryCount, context) =>
                         {
                             _logger.LogWarning("Retry attempt {RetryCount} due to: {Exception}", retryCount, exception);
                         });
        }

        /// <summary>
        /// Adds a new transaction asynchronously.
        /// This method uses a retry policy to handle transient failures.
        /// </summary>
        /// <param name="transaction">The transaction command to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task AddTransactionAsync(CreateTransactionCommand transaction)
        {
            await ExecuteWithRetryAsync(() => AddTransactionInternalAsync(transaction));
        }

        private async Task AddTransactionInternalAsync(CreateTransactionCommand transaction)
        {
            var addTask = _transactionRepository.AddTransactionAsync(transaction);
            var publishTask = _messagePublisher.PublishAsync("transactionQueue", transaction);
            await Task.WhenAll(addTask, publishTask);
        }
    }
}