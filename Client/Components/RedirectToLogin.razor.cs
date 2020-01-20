using Microsoft.AspNetCore.Components;

namespace MoneyManager.Client.Components
{
    public class RedirectToLogin : ComponentBase
    {
        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        protected override void OnInitialized()
        {
            NavManager.NavigateTo("/login");
        }
    }
}
