using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Web.Extensions;
using MoneyManager.Core.Services;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using MoneyManager.Core.Services.Exceptions;

namespace MoneyManager.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var acounts = await _accountService.ListAsyncNew();
            var accountDtos = acounts.Select(d => d.ToGetAccountDTO());
            return Ok(accountDtos);
        }


        [HttpPost]
        public async Task<IActionResult> PostAccount([FromBody] CreateAccountVm item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            try
            {
                var createdAccount = await _accountService.CreateAccountAsync(item);
                return Ok(createdAccount.ToGetAccountDTO());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, [FromBody] EditAccountVm item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            try
            {
                var createdAccount = await _accountService.EditAccountAsync(id, item);
                return Ok(createdAccount.ToGetAccountDTO());
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
                await _accountService.DeleteAsync(id);
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
