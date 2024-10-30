using Microsoft.AspNetCore.Mvc;
using Consolidado.Application.Repositories;

namespace Consolidado.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsolidationController : ControllerBase
    {
        private readonly IConsolidationRepository _repository;

        public ConsolidationController(IConsolidationRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetConsolidatedBalance()
        {
            var balance = await _repository.GetConsolidatedBalanceAsync();
            return balance != null ? Ok(balance) : NotFound();
        }
    }
}
