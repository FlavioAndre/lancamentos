using ControleLancamentos.Infrastructure.Messaging;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using Xunit;

namespace ControleLancamentos.Tests.Infrastructure.Messaging
{
    public class RabbitMqMessagePublisherTest : IDisposable
    {
        private readonly Mock<IConnectionFactory> _mockConnectionFactory;
        private readonly Mock<IConnection> _mockConnection;
        private readonly Mock<IModel> _mockChannel;
        private readonly Mock<ILogger<RabbitMqMessagePublisher>> _mockLogger;
        private readonly RabbitMqMessagePublisher _publisher;

        public RabbitMqMessagePublisherTest()
        {
            _mockConnectionFactory = new Mock<IConnectionFactory>();
            _mockConnection = new Mock<IConnection>();
            _mockChannel = new Mock<IModel>();
            _mockLogger = new Mock<ILogger<RabbitMqMessagePublisher>>();

            _mockConnectionFactory.Setup(cf => cf.CreateConnection()).Returns(_mockConnection.Object);
            _mockConnection.Setup(c => c.CreateModel()).Returns(_mockChannel.Object);

            _publisher = new RabbitMqMessagePublisher(_mockConnectionFactory.Object, _mockLogger.Object);
        }

        [Fact]
        public void Dispose_ShouldDisposeChannelAndConnection()
        {
            // Act
            _publisher.Dispose();

            // Assert
            _mockChannel.Verify(c => c.Dispose(), Times.Once);
            _mockConnection.Verify(c => c.Dispose(), Times.Once);
        }

        public void Dispose()
        {
            _publisher.Dispose();
        }
    }
}