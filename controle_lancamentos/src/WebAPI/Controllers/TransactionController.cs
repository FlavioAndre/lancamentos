using ControleLancamentos.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ControleLancamentos.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IMediator mediator, ILogger<TransactionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
        {
            await _mediator.Send(command);
            return Ok("Transaction created successfully.");
        }


    }
}
