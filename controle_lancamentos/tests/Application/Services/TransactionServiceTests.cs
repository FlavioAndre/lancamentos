using ControleLancamentos.Application.Commands;
using ControleLancamentos.Application.Interfaces;
using ControleLancamentos.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Polly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ControleLancamentos.Application.Tests.Services
{
    public class TransactionServiceTest
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly Mock<IMessagePublisher> _messagePublisherMock;
        private readonly Mock<ILogger<TransactionService>> _loggerMock;
        private readonly TransactionService _transactionService;

        public TransactionServiceTest()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _messagePublisherMock = new Mock<IMessagePublisher>();
            _loggerMock = new Mock<ILogger<TransactionService>>();
            _transactionService = new TransactionService(
                _transactionRepositoryMock.Object,
                _messagePublisherMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task AddTransactionAsync_Should_Call_AddAsync_And_PublishAsync()
        {
            // Arrange
            var transaction = new CreateTransactionCommand
            {
                Description = "Test Transaction",
                Type = "credit"
            };

            _transactionRepositoryMock.Setup(repo => repo.AddTransactionAsync(It.IsAny<CreateTransactionCommand>()))
                .Returns(Task.CompletedTask);
            _messagePublisherMock.Setup(pub => pub.PublishAsync(It.IsAny<string>(), It.IsAny<CreateTransactionCommand>()))
                .Returns(Task.CompletedTask);

            // Act
            await _transactionService.AddTransactionAsync(transaction);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.AddTransactionAsync(transaction), Times.Once);
            _messagePublisherMock.Verify(pub => pub.PublishAsync("transactionQueue", transaction), Times.Once);
        }

        [Fact]
        public async Task AddTransactionAsync_Should_Retry_On_Failure()
        {
            // Arrange
             var transaction = new CreateTransactionCommand
            {
                Description = "Test Transaction",
                Type = "credit"
            };

            var retryCount = 0;

            _transactionRepositoryMock.Setup(repo => repo.AddTransactionAsync(It.IsAny<CreateTransactionCommand>()))
                .Returns(() =>
                {
                    if (retryCount < 2)
                    {
                        retryCount++;
                        throw new Exception("Transient error");
                    }
                    return Task.CompletedTask;
                });

            _messagePublisherMock.Setup(pub => pub.PublishAsync(It.IsAny<string>(), It.IsAny<CreateTransactionCommand>()))
                .Returns(Task.CompletedTask);

            // Act
            await _transactionService.AddTransactionAsync(transaction);

            // Assert
            _transactionRepositoryMock.Verify(repo => repo.AddTransactionAsync(transaction), Times.Exactly(3));
            _messagePublisherMock.Verify(pub => pub.PublishAsync("transactionQueue", transaction), Times.Once);
        }
    }
}