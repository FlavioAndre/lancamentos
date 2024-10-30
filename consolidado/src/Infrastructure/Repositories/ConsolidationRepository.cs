using Consolidado.Application.Repositories;
using Consolidado.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Consolidado.Infrastructure.Data;
using ControleLancamentos.Application.Events;

namespace Consolidado.Infrastructure.Repositories
{
    public class ConsolidationRepository : IConsolidationRepository
    {
        private readonly ConsolidationDbContext _context;
        private readonly ILogger<ConsolidationRepository> _logger;

        public ConsolidationRepository(ConsolidationDbContext context, ILogger<ConsolidationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task UpdateBalanceAsync(TransactionEvent transactionEvent)
        {
            _logger.LogInformation("Atualizando saldo consolidado.");
            var consolidation = await _context.ConsolidatedBalances
                   .OrderByDescending(b => b.LastUpdated)
                   .FirstOrDefaultAsync() ?? new ConsolidatedBalance();


            var typeLower = transactionEvent.Type.ToLower();
            if (typeLower== "credit")
            {
                consolidation.TotalCredit += transactionEvent.Amount;
                consolidation.Balance += transactionEvent.Amount;
            }
            else if (typeLower == "debit")
            {
                consolidation.TotalDebit += transactionEvent.Amount;
                consolidation.Balance -= transactionEvent.Amount;
            }

            consolidation.LastUpdated = DateTime.UtcNow;

            if (_context.ConsolidatedBalances.Contains(consolidation))
            {
                _context.ConsolidatedBalances.Update(consolidation);
            }
            else
            {
                await _context.ConsolidatedBalances.AddAsync(consolidation);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Consolidated balance updated: Balance={Balance}", consolidation.Balance);
        }

        public async Task<ConsolidatedBalance> GetConsolidatedBalanceAsync()
        {
            _logger.LogInformation("Obtendo o saldo consolidado mais recente.");

            var consolidatedBalance = await _context.ConsolidatedBalances
                .OrderByDescending(b => b.LastUpdated)
                .FirstOrDefaultAsync();

            if (consolidatedBalance == null)
            {
                _logger.LogInformation("Nenhum saldo consolidado encontrado. Retornando valores iniciais.");
                return new ConsolidatedBalance
                {
                    TotalCredit = 0,
                    TotalDebit = 0,
                    Balance = 0,
                    LastUpdated = DateTime.UtcNow
                };
            }

            _logger.LogInformation("Saldo consolidado obtido com sucesso: Créditos = {TotalCredits}, Débitos = {TotalDebits}, Saldo = {Balance}",
                consolidatedBalance.TotalCredit, consolidatedBalance.TotalDebit, consolidatedBalance.Balance);

            return consolidatedBalance;
        }
    }
}
