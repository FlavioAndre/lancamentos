using Consolidado.Infrastructure.Data;
using Consolidado.Application.Events;
using Consolidado.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Consolidado.Infrastructure.Repositories.Tests
{
    public class ConsolidationRepositoryTest
    {
        private readonly ConsolidationDbContext _context;
        private readonly Mock<ILogger<ConsolidationRepository>> _mockLogger;
        private readonly ConsolidationRepository _repository;

        public ConsolidationRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ConsolidationDbContext>()
                .UseInMemoryDatabase(databaseName: "ConsolidationTestDb")
                .Options;

            _context = new ConsolidationDbContext(options);
            _mockLogger = new Mock<ILogger<ConsolidationRepository>>();
            _repository = new ConsolidationRepository(_context, _mockLogger.Object);
        }

        [Fact]
        public async Task UpdateBalanceAsync_ShouldUpdateBalance_WhenTransactionIsCredit()
        {
            // Arrange
            var transactionEvent = new TransactionEvent(100, "credit", DateTime.UtcNow);
            var consolidatedBalance = new ConsolidatedBalance { TotalCredit = 0, TotalDebit = 0, Balance = 0, LastUpdated = DateTime.UtcNow };

            _context.ConsolidatedBalances.Add(consolidatedBalance);
            await _context.SaveChangesAsync();

            // Act
            await _repository.UpdateBalanceAsync(transactionEvent);

            // Assert
            Assert.Equal(100, consolidatedBalance.TotalCredit);
            Assert.Equal(100, consolidatedBalance.Balance);
            _context.ConsolidatedBalances.Update(consolidatedBalance);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task UpdateBalanceAsync_ShouldUpdateBalance_WhenTransactionIsDebit()
        {
            // Arrange
            var transactionEvent = new TransactionEvent(50, "debit", DateTime.UtcNow);
            var consolidatedBalance = new ConsolidatedBalance { TotalCredit = 0, TotalDebit = 0, Balance = 100, LastUpdated = DateTime.UtcNow };

            _context.ConsolidatedBalances.Add(consolidatedBalance);
            await _context.SaveChangesAsync();

            // Act
            await _repository.UpdateBalanceAsync(transactionEvent);

            // Assert
            Assert.Equal(50, consolidatedBalance.TotalDebit);
            Assert.Equal(50, consolidatedBalance.Balance);
            _context.ConsolidatedBalances.Update(consolidatedBalance);
            await _context.SaveChangesAsync();
        }

        [Fact]
        public async Task GetConsolidatedBalanceAsync_ShouldReturnInitialValues_WhenNoBalanceExists()
        {
            // Act
            var result = await _repository.GetConsolidatedBalanceAsync();

            // Assert
            Assert.Equal(100, result.TotalCredit);
            Assert.Equal(0, result.TotalDebit);
            Assert.Equal(100, result.Balance);
        }

        [Fact]
        public async Task GetConsolidatedBalanceAsync_ShouldReturnLatestBalance_WhenBalanceExists()
        {
            // Arrange
            var consolidatedBalance = new ConsolidatedBalance { TotalCredit = 100, TotalDebit = 50, Balance = 50, LastUpdated = DateTime.UtcNow };

            _context.ConsolidatedBalances.Add(consolidatedBalance);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetConsolidatedBalanceAsync();

            // Assert
            Assert.Equal(100, result.TotalCredit);
            Assert.Equal(50, result.TotalDebit);
            Assert.Equal(50, result.Balance);
        }
    }
}