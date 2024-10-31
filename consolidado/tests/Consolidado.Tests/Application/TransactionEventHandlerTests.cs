using Consolidado.Application.Handlers;
using Consolidado.Application.Repositories;
using Consolidado.Application.Events;
using Microsoft.Extensions.Logging;
using Moq;

namespace Consolidado.Application.Tests.Handlers
{
    public class TransactionEventHandlerTest
    {
        private readonly Mock<IConsolidationRepository> _mockConsolidationRepository;
        private readonly Mock<ILogger<TransactionEventHandler>> _mockLogger;
        private readonly TransactionEventHandler _handler;

        public TransactionEventHandlerTest()
        {
            _mockConsolidationRepository = new Mock<IConsolidationRepository>();
            _mockLogger = new Mock<ILogger<TransactionEventHandler>>();
            _handler = new TransactionEventHandler(_mockConsolidationRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task HandleTransactionEvent_ShouldLogInformationAndCallUpdateBalanceAsync()
        {
            // Arrange
            var transactionEvent = new TransactionEvent(100M, "Credit", DateTime.Now);

            // Act
            await _handler.HandleTransactionEvent(transactionEvent);

            // Assert
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null &&
                        v.ToString().Contains("Recebendo evento de transação: Amount=") && 
                        v.ToString().Contains("Type=")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);


            _mockConsolidationRepository.Verify(x => x.UpdateBalanceAsync(transactionEvent), Times.Once);

            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v != null 
                        && v.ToString().Contains("Transação atualizada com sucesso: Amount=") 
                        && v.ToString().Contains("Type=")),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
#pragma warning restore CS8602 // Dereference of a possibly null reference.                
        }
    }
}