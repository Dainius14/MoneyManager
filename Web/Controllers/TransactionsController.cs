using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Web.Extensions;
using MoneyManager.Core.Services;
using System.Linq;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.DTO;

namespace money_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionsService;

        public TransactionsController(ITransactionService transactionService)
        {
            _transactionsService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await _transactionsService.ListAsync();

            var transactionsDto = transactions.Item!.Select(t => t.ToGetTransactionDTO());
            return Ok(transactionsDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] EditTransactionDTO inputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }
            var transaction = inputDto.ToDomainModel();
            var response = await _transactionsService.CreateAsync(transaction);

            if (!response.Success)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var transactionDto = response.Item!.ToGetTransactionDTO();
            return Ok(transactionDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, [FromBody] EditTransactionDTO inputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var transaction = inputDto.ToDomainModel();
            var result = await _transactionsService.UpdateAsync(id, transaction);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var transactionDto = result.Item!.ToGetTransactionDTO();
            return Ok(transactionDto);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _transactionsService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            return NoContent();
        }
    }
}
