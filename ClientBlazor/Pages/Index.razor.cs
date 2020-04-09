using Microsoft.AspNetCore.Components;

namespace MoneyManager.Client.Pages
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        protected override void OnInitialized()
        {
            NavManager.NavigateTo("/transactions");
        }
    }
}
