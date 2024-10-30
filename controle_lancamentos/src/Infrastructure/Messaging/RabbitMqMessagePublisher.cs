using ControleLancamentos.Application.Commands;
using ControleLancamentos.Application.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ControleLancamentos.Infrastructure.Messaging
{
    public class RabbitMqMessagePublisher : IMessagePublisher, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMqMessagePublisher> _logger;

        public RabbitMqMessagePublisher(IConnectionFactory connectionFactory, ILogger<RabbitMqMessagePublisher> logger)
        {
            _logger = logger;
            _connection = connectionFactory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task PublishAsync(string queueName, CreateTransactionCommand command)
        {
            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(command));
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            await Task.Run(() => _channel.BasicPublish("", queueName, null, messageBody));
            _logger.LogInformation("Mensagem publicada na fila: {QueueName}", queueName);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
