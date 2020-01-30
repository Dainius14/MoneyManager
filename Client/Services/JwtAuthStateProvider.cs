using Microsoft.AspNetCore.Components.Authorization;
using MoneyManager.Models.Domain;
using MoneyManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private ClaimsPrincipal? _principal;
        private AuthService _authService;

        public JwtAuthStateProvider(AuthService authService)
        {
            _authService = authService;

            _authService.OnAuthenticated += SetAuthState;
        }

        private void _authService_OnAuthenticated(string accessToken)
        {
            throw new NotImplementedException();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_principal != null)
            {
                return new AuthenticationState(_principal);
            }

            if (!await _authService.TryAuthenticate())
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            var payload = _authService.GetJwtPayload(_authService.GetAccessTokenFromSessionStorage()!);
            var claims = GetClaimsFromPayload(payload);
            var identity = new ClaimsIdentity(claims, "jwt");
            _principal = new ClaimsPrincipal(identity);
            return new AuthenticationState(_principal);
        }

        private void SetAuthState(Dictionary<string, object> jwtPayload)
        {
            var claims = GetClaimsFromPayload(jwtPayload);
            var identity = new ClaimsIdentity(claims, "jwt");
            _principal = new ClaimsPrincipal(identity);
            var authState = Task.FromResult(new AuthenticationState(_principal));
            NotifyAuthenticationStateChanged(authState);
        }

        public void ResetAuthState()
        {
            var noUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(noUser));
            NotifyAuthenticationStateChanged(authState);
        }

        private static IEnumerable<Claim> GetClaimsFromPayload(Dictionary<string, object> payload)
        {
            var claims = new List<Claim>();

            payload.TryGetValue(ClaimTypes.Role, out object roles);

            if (roles != null)
            {
                if (roles.ToString().Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());

                    foreach (var parsedRole in parsedRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                    }
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                }

                payload.Remove(ClaimTypes.Role);
            }

            claims.AddRange(
                payload.Select(kvp =>
                    new Claim(kvp.Key, kvp.Value.ToString())
                )
            );

            return claims;
        }

        private Dictionary<string, object> GetJwtPayload(string jwt)
        {
            var claims = new List<Claim>();
            string payload = jwt.Split('.')[1];
            byte[] jsonBytes = ParseBase64WithoutPadding(payload);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

    }
}
