using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;

namespace MoneyManager.Core.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public int Id
        {
            get
            {
                try
                {
                    return int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
