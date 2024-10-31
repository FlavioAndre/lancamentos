using ControleLancamentos.Application.Commands;

namespace ControleLancamentos.Application.Interfaces
{
    public interface ITransactionService
    {
        /// <summary>
        /// Adds a new transaction asynchronously.
        /// </summary>
        /// <param name="transaction">The transaction command containing the details of the transaction to be added.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddTransactionAsync(CreateTransactionCommand transaction);
    }
}