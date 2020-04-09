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
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData([FromQuery]DateTime fromDate, [FromQuery]DateTime toDate)
        {
            var data = await _dashboardService.GatherDataAsync(fromDate, toDate);
            return Ok(data);
        }
    }
}
