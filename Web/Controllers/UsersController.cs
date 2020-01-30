using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Core.Services;
using MoneyManager.Core.Services.Exceptions;
using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.ViewModels;
using MoneyManager.Web.Helpers;

namespace MoneyManager.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;
        private readonly AppSettings _appSettings;

        public UsersController(IUserService userService, IOptions<AppSettings> appSettings)
        {
            _userService = userService;
            _appSettings = appSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateDto authDto)
        {
            User user;
            try
            {
                user = await _userService.Authenticate(authDto.Email, authDto.Password);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(new { ex.Message });
            }
            catch (BadUserPasswordException ex)
            {
                return BadRequest(new { ex.Message });
            }

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Id.ToString())
            };
            string accessToken = AuthHelper.GenerateAccessToken(claims, _appSettings.Secret);
            string refreshToken = AuthHelper.GenerateRefreshToken();

            await _userService.SaveRefreshToken((int)user.Id!, refreshToken);

            return Ok(new AuthenticatedUserVm(user, accessToken, refreshToken).ToDto());
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]EditUserDto vm)
        {
            var user = vm.ToDomainModel();

            try
            {
                bool result = await _userService.Create(user);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return BadRequest("bbz");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("refreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenVmDto dto)
        {
            var requestTokens = dto.ToViewModel();
            ClaimsPrincipal principal;
            try
            {
                principal = AuthHelper.GetPrincipalFromExpiredToken(dto.AccessToken, _appSettings.Secret);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized(new { Message = "Invalid auth token" });
            }


            if (!await _userService.IsRefreshTokenValid(requestTokens.RefreshToken))
            {
                return Unauthorized(new { Message = "Invalid refresh token" });
            }

            var newAccessToken = AuthHelper.GenerateAccessToken(principal.Claims, _appSettings.Secret);
            var newRefreshToken = AuthHelper.GenerateRefreshToken();

            var userId = Convert.ToInt32(principal.Identity.Name);
            await _userService.InvalidateRefreshToken(requestTokens.RefreshToken);
            await _userService.SaveRefreshToken(userId, newRefreshToken);
            var user = await _userService.GetOne(userId);

            return Ok(new AuthenticatedUserVm(user, newAccessToken, newRefreshToken).ToDto());
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.Name));
            var user = await _userService.GetOne(userId);
            return Ok(user.ToGetUserDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAll();
                var usersDto = users.Select(u => u.ToGetUserDto());
                return Ok(usersDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}