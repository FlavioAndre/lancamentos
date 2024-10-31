using System.Text;
using System.Text.Json;
using Consolidado.Application.Events;
using Consolidado.Application.Interfaces;
using Consolidado.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Consolidado.Tests.Infrastructure.Messaging
{
    public class RabbitMqTransactionConsumerTest
    {
        private readonly Mock<IConnectionFactory> _mockConnectionFactory;
        private readonly Mock<ILogger<RabbitMqTransactionConsumer>> _mockLogger;
        private readonly Mock<IServiceScopeFactory> _mockServiceScopeFactory;
        private readonly Mock<IServiceScope> _mockServiceScope;
        private readonly Mock<ITransactionEventHandler> _mockTransactionEventHandler;
        private readonly Mock<IModel> _mockChannel;
        private readonly Mock<IConnection> _mockConnection;

        public RabbitMqTransactionConsumerTest()
        {
            _mockConnectionFactory = new Mock<IConnectionFactory>();
            _mockLogger = new Mock<ILogger<RabbitMqTransactionConsumer>>();
            _mockServiceScopeFactory = new Mock<IServiceScopeFactory>();
            _mockServiceScope = new Mock<IServiceScope>();
            _mockTransactionEventHandler = new Mock<ITransactionEventHandler>();
            _mockChannel = new Mock<IModel>();
            _mockConnection = new Mock<IConnection>();

            _mockConnectionFactory.Setup(cf => cf.CreateConnection()).Returns(_mockConnection.Object);
            _mockConnection.Setup(c => c.CreateModel()).Returns(_mockChannel.Object);
            _mockServiceScopeFactory.Setup(ssf => ssf.CreateScope()).Returns(_mockServiceScope.Object);
            _mockServiceScope.Setup(ss => ss.ServiceProvider.GetService(typeof(ITransactionEventHandler)))
                .Returns(_mockTransactionEventHandler.Object);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldConsumeMessageAndProcessEvent()
        {
            // Arrange
            var consumer = new RabbitMqTransactionConsumer(
                _mockConnectionFactory.Object,
                _mockLogger.Object,
                _mockServiceScopeFactory.Object);

            var stoppingToken = new CancellationTokenSource().Token;

            var message = new TransactionEvent(100.0m, "credit", DateTime.UtcNow);
            var messageBody = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
            var eventArgs = new BasicDeliverEventArgs
            {
                Body = messageBody
            };

            var eventingBasicConsumer = new EventingBasicConsumer(_mockChannel.Object);
            eventingBasicConsumer.Received += (model, ea) =>
            {
                _mockTransactionEventHandler.Object.HandleTransactionEvent(message);
                _mockChannel.Object.BasicAck(ea.DeliveryTag, false);
            };

            // Act
            await consumer.StartAsync(stoppingToken);

            // Simulate message delivery
            eventingBasicConsumer.HandleBasicDeliver("consumerTag", 1, false, "exchange", "routingKey", null, eventArgs.Body);

            // Assert
            _mockTransactionEventHandler.Verify(handler => handler.HandleTransactionEvent(It.IsAny<TransactionEvent>()), Times.Once);
            _mockChannel.Verify(c => c.BasicAck(It.IsAny<ulong>(), false), Times.Once);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldLogError_WhenExceptionOccurs()
        {
            // Arrange
            var consumer = new RabbitMqTransactionConsumer(
                _mockConnectionFactory.Object,
                _mockLogger.Object,
                _mockServiceScopeFactory.Object);

            var stoppingToken = new CancellationTokenSource().Token;

            _mockConnectionFactory.Setup(cf => cf.CreateConnection()).Throws(new Exception("Connection error"));

            // Act
            await consumer.StartAsync(stoppingToken);

            // Assert
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockLogger.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null && v.ToString().Contains("Erro ao conectar ao RabbitMQ")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }
    }
}