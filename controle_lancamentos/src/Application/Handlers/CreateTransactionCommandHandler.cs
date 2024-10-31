using ControleLancamentos.Application.Commands;
using ControleLancamentos.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ControleLancamentos.Application.Handlers
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, Unit>
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<CreateTransactionCommandHandler> _logger;

        public CreateTransactionCommandHandler(ITransactionService transactionService, ILogger<CreateTransactionCommandHandler> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Manipulando o comando {CommandName} - {@Command}", nameof(CreateTransactionCommand), command);

            try
            {
                await _transactionService.AddTransactionAsync(command);
                _logger.LogInformation("Comando {CommandName} manipulado com sucesso - {@Command}", nameof(CreateTransactionCommand), command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao manipular o comando {CommandName} - {@Command}", nameof(CreateTransactionCommand), command);
                throw;
            }
            return Unit.Value;
        }


    }

}