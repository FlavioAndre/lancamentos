using ControleLancamentos.Application.Commands;
using ControleLancamentos.Application.Interfaces;
using MediatR;

namespace ControleLancamentos.Application.Handlers
{
    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand>
    {
        private readonly ITransactionService _transactionRepository;

        public CreateTransactionCommandHandler(ITransactionService transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Unit> Handle(CreateTransactionCommand command, CancellationToken cancellationToken)
        {
            await _transactionRepository.AddTransactionAsync(command);

            return Unit.Value;
        }

       
    }

}