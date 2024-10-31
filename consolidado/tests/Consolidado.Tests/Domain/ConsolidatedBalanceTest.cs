namespace Consolidado.Domain.Entities.Tests
{
    public class ConsolidatedBalanceTest
    {
        [Fact]
        public void ConsolidatedBalance_ShouldInitializeWithDefaultValues()
        {
            // Arrange & Act
            var consolidatedBalance = new ConsolidatedBalance();

            // Assert
            Assert.Equal(0, consolidatedBalance.Id);
            Assert.Equal(0.00M, consolidatedBalance.TotalCredit);
            Assert.Equal(0.00M, consolidatedBalance.TotalDebit);
            Assert.Equal(0.00M, consolidatedBalance.Balance);
            Assert.True((DateTime.UtcNow - consolidatedBalance.LastUpdated).TotalSeconds < 1);
        }

        [Fact]
        public void ConsolidatedBalance_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var consolidatedBalance = new ConsolidatedBalance();
            var now = DateTime.UtcNow;

            // Act
            consolidatedBalance.Id = 1;
            consolidatedBalance.TotalCredit = 100.00M;
            consolidatedBalance.TotalDebit = 50.00M;
            consolidatedBalance.Balance = 50.00M;
            consolidatedBalance.LastUpdated = now;

            // Assert
            Assert.Equal(1, consolidatedBalance.Id);
            Assert.Equal(100.00M, consolidatedBalance.TotalCredit);
            Assert.Equal(50.00M, consolidatedBalance.TotalDebit);
            Assert.Equal(50.00M, consolidatedBalance.Balance);
            Assert.Equal(now, consolidatedBalance.LastUpdated);
        }
    }
}