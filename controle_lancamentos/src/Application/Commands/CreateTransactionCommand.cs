using MediatR;

namespace ControleLancamentos.Application.Commands
{
    public class CreateTransactionCommand : IRequest
    {
        public decimal Amount { get; set; }
        public required string Type { get; set; }
        public required string Description { get; set; }
    }
}