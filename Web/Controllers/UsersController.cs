using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UsersController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
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
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            string accessToken = AuthHelper.GenerateAccessToken(claims, _configuration["App:Secret"]);
            string refreshToken = AuthHelper.GenerateRefreshToken();

            await _userService.SaveRefreshToken((int)user.Id!, refreshToken);

            return Ok(new AuthenticatedUserVm(user, accessToken, refreshToken).ToDto());
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody]RegisterUserVmDto vm)
        {
            var user = vm.ToViewModel();

            try
            {
                var createdUser = await _userService.Create(user);

                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, createdUser.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                };
                string accessToken = AuthHelper.GenerateAccessToken(claims, _configuration["App:Secret"]);
                string refreshToken = AuthHelper.GenerateRefreshToken();

                await _userService.SaveRefreshToken((int)createdUser.Id!, refreshToken);

                return Ok(new AuthenticatedUserVm(createdUser, accessToken, refreshToken).ToDto());
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
                principal = AuthHelper.GetPrincipalFromExpiredToken(dto.AccessToken, _configuration["App:Secret"]);
            }
            catch (SecurityTokenException)
            {
                return Unauthorized(new { Message = "Invalid auth token" });
            }


            if (!await _userService.IsRefreshTokenValid(requestTokens.RefreshToken))
            {
                return Unauthorized(new { Message = "Invalid refresh token" });
            }

            var newAccessToken = AuthHelper.GenerateAccessToken(principal.Claims, _configuration["App:Secret"]);
            var newRefreshToken = AuthHelper.GenerateRefreshToken();

            var userId = Convert.ToInt32(principal.FindFirst(ClaimTypes.NameIdentifier).Value);
            await _userService.InvalidateRefreshToken(requestTokens.RefreshToken);
            await _userService.SaveRefreshToken(userId, newRefreshToken);
            var user = await _userService.GetOne(userId);

            return Ok(new AuthenticatedUserVm(user, newAccessToken, newRefreshToken).ToDto());
        }

        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
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