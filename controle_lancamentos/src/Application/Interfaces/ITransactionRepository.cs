using ControleLancamentos.Application.Commands;

namespace ControleLancamentos.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(CreateTransactionCommand transaction);
    }
}
