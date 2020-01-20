using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoneyManager.Core.Services;
using MoneyManager.Core.Services.Exceptions;
using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.ViewModels;

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

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandler.WriteToken(token);

            return Ok(new AuthenticatedUserVm(user, tokenStr).ToDto());
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