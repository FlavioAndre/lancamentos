using Xunit;
using Moq;
using ControleLancamentos.Application.Services;
using ControleLancamentos.Application.Interfaces;
using ControleLancamentos.Application.Commands;
using ControleLancamentos.Domain.ValueObjects;
using ControleLancamentos.Domain.Entities;
using ControleLancamentos.Infrastructure.Data;

using System.Collections.Generic;
using Microsoft.Extensions.Logging;

public class TransactionServiceTests
{
    private readonly TransactionService _service;
    private readonly Mock<ITransactionRepository> _repositoryMock;
    private readonly Mock<IMessagePublisher> _messagePublisherMock;
    private readonly Mock<ILogger<TransactionService>> _loggerMock;

    public TransactionServiceTests()
    {
        _repositoryMock = new Mock<ITransactionRepository>();
        _messagePublisherMock = new Mock<IMessagePublisher>();
        _loggerMock = new Mock<ILogger<TransactionService>>();
        
        _service = new TransactionService(_repositoryMock.Object, _messagePublisherMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void AdicionarCredito_DeveAtualizarSaldo()
    {
        var transaction = new CreateTransactionCommand { Type = "credit", Amount = 100.0M, Description = "Depósito"};
        _service.AddTransactionAsync(transaction);

        _repositoryMock.Verify(repo => repo.AddAsync(transaction), Times.Once);
    }

    [Fact]
    public void AdicionarDebito_DeveAtualizarSaldo()
    {
        var transaction = new CreateTransactionCommand { Type = "debit", Amount = 50.0M , Description = "Depósito"};
        _service.AddTransactionAsync(transaction);

        _repositoryMock.Verify(repo => repo.AddAsync(transaction), Times.Once);
    }

}
