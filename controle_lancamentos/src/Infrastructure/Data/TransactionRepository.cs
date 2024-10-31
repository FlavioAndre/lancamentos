using ControleLancamentos.Application.Commands;
using ControleLancamentos.Application.Interfaces;
using ControleLancamentos.Domain.Entities;
using ControleLancamentos.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace ControleLancamentos.Infrastructure.Data
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDbContext _context;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(TransactionDbContext context, ILogger<TransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddTransactionAsync(CreateTransactionCommand command)
        {
            var transaction = new Transaction
            {
                Amount = command.Amount,
                Type = TransactionType.FromString(command.Type),
                Description = command.Description
            };
            
            if (transaction.CreatedAt == default)
            {
                transaction.CreatedAt = DateTime.UtcNow;
            }

            _logger.LogInformation("Adicionando transação: Amount = {Amount}, Type = {Type}, Description = {Description}, CreatedAt = {CreatedAt}",
            transaction.Amount, transaction.Type, transaction.Description, transaction.CreatedAt);

            await _context.Transactions.AddAsync(transaction);

            try
            {
                var result = await _context.SaveChangesAsync();
                _logger.LogInformation("Transação salva com sucesso no banco de dados. Linhas afetadas: {Result}", result);

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao salvar transação no banco de dados: {Message}", ex.Message);
                throw;
            }
        }
    }
}
