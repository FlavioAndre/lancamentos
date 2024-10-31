using Moq;
using Microsoft.AspNetCore.Mvc;
using Consolidado.Application.Repositories;
using Consolidado.WebAPI.Controllers;
using Consolidado.Domain.Entities;

namespace Consolidado.WebAPI.Tests.Controllers
{
    public class ConsolidationControllerTest
    {
        private readonly Mock<IConsolidationRepository> _mockRepository;
        private readonly ConsolidationController _controller;

        public ConsolidationControllerTest()
        {
            _mockRepository = new Mock<IConsolidationRepository>();
            _controller = new ConsolidationController(_mockRepository.Object);
        }

        [Fact]
        public async Task GetConsolidatedBalance_ReturnsOkResult_WhenBalanceIsNotNull()
        {
            // Arrange
            var balance = new ConsolidatedBalance { TotalCredit = 100, TotalDebit = 50, Balance = 50, LastUpdated = DateTime.UtcNow  };
            _mockRepository.Setup(repo => repo.GetConsolidatedBalanceAsync()).ReturnsAsync(balance);

            // Act
            var result = await _controller.GetConsolidatedBalance();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(balance, okResult.Value);
        }
       
    }
}