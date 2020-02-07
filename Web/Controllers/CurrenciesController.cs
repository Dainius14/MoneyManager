using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Core.Services;
using MoneyManager.Models.Mappers;

namespace MoneyManager.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CurrenciesController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrenciesController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _currencyService.ListAsync();
            var itemsDto = items.Select(d => d.ToGetCurrencyDTO());
            return Ok(itemsDto);
        }
    }
}
