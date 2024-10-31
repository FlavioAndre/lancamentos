using ControleLancamentos.Application.Commands;

namespace ControleLancamentos.Application.Interfaces
{
    public interface IMessagePublisher
    {      
        /// <summary>
        /// Publishes a transaction command to the specified destination queue.
        /// </summary>
        /// <param name="destinationQueueName">The name of the destination queue.</param>
        /// <param name="transaction">The transaction command to be published.</param>
        /// <returns>A task that represents the asynchronous publish operation.</returns>
        Task PublishAsync(string destinationQueueName, CreateTransactionCommand transaction);
        
    }
}
