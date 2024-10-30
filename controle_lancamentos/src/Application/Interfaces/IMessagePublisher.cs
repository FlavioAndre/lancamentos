using ControleLancamentos.Application.Commands;

namespace ControleLancamentos.Application.Interfaces
{
    public interface IMessagePublisher
    {
        Task PublishAsync(string queueName, CreateTransactionCommand transaction);
    }
}
