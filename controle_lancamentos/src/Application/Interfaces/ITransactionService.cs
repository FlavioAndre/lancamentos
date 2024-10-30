using ControleLancamentos.Application.Commands;

namespace ControleLancamentos.Application.Interfaces
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(CreateTransactionCommand transaction);
    }
}