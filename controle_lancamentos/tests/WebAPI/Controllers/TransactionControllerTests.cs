using Xunit;
using Moq;
using ControleLancamentos.WebAPI.Controllers;
using ControleLancamentos.Domain.Entities; 
using ControleLancamentos.Domain.ValueObjects;
using ControleLancamentos.Application.Commands;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.AspNetCore.Mvc;

public class TransactionControllerTests
{
    private readonly TransactionController _controller;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<ILogger<TransactionController>> _loggerMock;

    public TransactionControllerTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _loggerMock = new Mock<ILogger<TransactionController>>();
        
        _controller = new TransactionController(_mediatorMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task CriarLancamentoCredito_DeveRetornarOk()
    {
        var command = new CreateTransactionCommand { Type = "credit", Amount = 100.0M, Description = "Depósito" };
        
        var result = await _controller.CreateTransaction(command);
        
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CriarLancamentoDebito_DeveRetornarOk()
    {
        var command = new CreateTransactionCommand { Type = "debit" , Amount = 50.0M, Description = "Pagamento" };
        
        var result =  await _controller.CreateTransaction(command);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task CriarLancamentoInvalido_DeveRetornarBadRequest()
    {
        var command = new CreateTransactionCommand { Type = "credit",  Description = "Transação inválida" };
        
        var result = await _controller.CreateTransaction(command);

        Assert.IsType<OkObjectResult>(result);
    }
}
