using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Web.Extensions;
using MoneyManager.Core.Services;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace MoneyManager.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var acounts = await _accountService.ListAsync();
            var accountDtos = acounts.Select(d => d.ToGetAccountDTO());
            return Ok(accountDtos);
        }

        [HttpGet("personal")]
        public async Task<IActionResult> GetAllPersonal()
        {
            var acounts = await _accountService.ListPersonalAsync();
            var accountDtos = acounts.Select(d => d.ToDto());
            return Ok(accountDtos);
        }

        [HttpGet("nonpersonal")]
        public async Task<IActionResult> GetAllNonPersonal()
        {
            var acounts = await _accountService.ListNonPersonalAsync();
            var accountDtos = acounts.Select(d => d.ToDto());
            return Ok(accountDtos);
        }

        [HttpPost("personal")]
        public async Task<IActionResult> PostPersonalAccount([FromBody] EditPersonalAccountVmDto dto)
        {
            var vm = dto.ToViewModel();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var response = await _accountService.CreateAsync(vm);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Item!.ToDto());
        }

        [HttpPost("nonpersonal")]
        public async Task<IActionResult> PostPersonalAccount([FromBody] EditNonPersonalAccountVmDto dto)
        {
            var vm = dto.ToViewModel();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var response = await _accountService.CreateAsync(vm);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response.Item!.ToDto());
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var result = await _accountService.GetById(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            if (result.Item == null)
            {
                return NotFound(id);
            }

            var accountDto = result.Item!.ToGetAccountDTO();
            return Ok(accountDto);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAccount(int id, [FromBody] EditPersonalAccountVmDto resource)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState.GetErrorMessages());
        //    }

        //    var account = resource.ToDomainModel();
        //    var result = await _accountService.UpdateAsync(id, account);

        //    if (!result.Success)
        //    {
        //        return BadRequest(result.Message);
        //    }

        //    if (result.Item == null)
        //    {
        //        return NotFound(id);
        //    }

        //    var accountDto = result.Item!.ToGetAccountDTO();
        //    return Ok(accountDto);
        //}

        //[HttpPost]
        //public async Task<IActionResult> PostAccount([FromBody] EditPersonalAccountDTO dto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState.GetErrorMessages());
        //    }

        //    var account = dto.ToDomainModel();
        //    var response = await _accountService.CreateAsync(account);

        //    if (!response.Success)
        //    {
        //        return BadRequest(ModelState.GetErrorMessages());
        //    }

        //    var accountDto = response.Item!.ToGetAccountDTO();
        //    return Ok(accountDto);
        //}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _accountService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return NoContent();
        }

    }
}
