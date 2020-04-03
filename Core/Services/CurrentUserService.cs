using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace MoneyManager.Core.Services
{
    public class CurrentUserService
    {
        public int Id { get; }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            Id = int.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
