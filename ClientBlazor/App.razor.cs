using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Services;

namespace MoneyManager.Client
{
    public class AppBase : ComponentBase
    {

        [Inject]
        protected HttpClient HttpClient { get; set; } = null!;

        [Inject]
        protected AuthService AuthService { get; set; } = null!;

        [Inject]
        protected LocalStorage LocalStorage { get; set; } = null!;

        [Inject]
        protected Bootstrapper Bootstrapper { get; set; } = null!;


        protected override void OnInitialized()
        {
            AuthService.OnAuthenticated += async (_) =>
            {
                await Bootstrapper.GetData();
            };
        }
        
    }
}
