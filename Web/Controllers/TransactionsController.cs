using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Web.Extensions;
using MoneyManager.Core.Services;
using System.Linq;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using System;
using MoneyManager.Core.Services.Exceptions;

namespace money_backend.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _transactionsService;

        public TransactionsController(TransactionService transactionService)
        {
            _transactionsService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var transactions = await _transactionsService.ListAsync();
                var transactionsDto = transactions.Select(t => t.ToGetTransactionDTO());
                return Ok(transactionsDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] EditTransactionDTO inputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var transaction = inputDto.ToDomainModel();
            try
            {
                var created = await _transactionsService.CreateAsync(transaction);
                var transactionDto = created.ToGetTransactionDTO();
                return Ok(transactionDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction(int id, [FromBody] EditTransactionDTO inputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var transaction = inputDto.ToDomainModel();
            try
            {
                var updated = await _transactionsService.UpdateAsync(id, transaction);
                var transactionDto = updated.ToGetTransactionDTO();
                return Ok(transactionDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _transactionsService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
