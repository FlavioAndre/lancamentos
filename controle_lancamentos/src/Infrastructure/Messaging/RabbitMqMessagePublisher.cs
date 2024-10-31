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
            if (!_connection.IsOpen || !_channel.IsOpen)
            {
                _logger.LogError("RabbitMQ connection or channel is closed.");
                throw new InvalidOperationException("RabbitMQ connection or channel is closed.");
            }

            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(command));
            DeclareQueue(queueName);
            await PublishMessageAsync(queueName, messageBody);
            _logger.LogInformation("Message published to queue: {QueueName}", queueName);
        }

        private void DeclareQueue(string queueName)
        {
            _channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false);
        }

        private Task PublishMessageAsync(string queueName, byte[] messageBody)
        {
            return Task.Run(() => _channel.BasicPublish("", queueName, null, messageBody));
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}