using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using MoneyManager.Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Client.Pages
{
    public class LoginBase : ComponentBase
    {
        [CascadingParameter]
        protected Task<AuthenticationState> _authStateTask { get; set; }

        [Inject]
        protected Bootstrapper Bootstrapper { get; set; } = null!;

        [Inject]
        protected AuthService AuthService { get; set; } = null!;

        //[Inject]
        //protected JwtAuthStateProvider AuthProvider { get; set; } = null!;

        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        protected string Email { get; set; } = string.Empty;
        protected string Password { get; set; } = string.Empty;

        protected bool CanLogin =>
            !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password);


        protected override async Task OnInitializedAsync()
        {
            var authState = await _authStateTask;
            if (authState.User.Identity.IsAuthenticated)
            {
                NavManager.NavigateTo("/transactions");
            }

        }


        protected async Task HandleLoginClick()
        {
            if (!CanLogin)
            {
                return;
            }

            if (await AuthService.AuthenticateAsync(Email, Password))
            {
                NavManager.NavigateTo("/transactions");
            }
        }

        protected async Task HandleKeyPress(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await HandleLoginClick();
            }
        }
    }
}
