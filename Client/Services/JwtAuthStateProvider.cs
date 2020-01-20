using Microsoft.AspNetCore.Components.Authorization;
using MoneyManager.Models.Domain;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private User? _user;
        public User? User
        {
            private get => _user;
            set
            {
                _user = value;
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity;
            if (User != null)
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, User.Email),
                }, "JwtAuth");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);
            return Task.FromResult(new AuthenticationState(user));
        }
    }
}
