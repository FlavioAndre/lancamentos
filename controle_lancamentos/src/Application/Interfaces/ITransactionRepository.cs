using ControleLancamentos.Application.Commands;

namespace ControleLancamentos.Application.Interfaces
{
    public interface ITransactionRepository
    {
        /// <summary>
        /// Asynchronously adds a new transaction.
        /// </summary>
        /// <param name="transaction">The transaction command to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task AddTransactionAsync(CreateTransactionCommand transaction);
    }
}
