using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Services;

namespace MoneyManager.Client.Components
{
    public class MessageComponentBase : ComponentBase
    {
        [Inject]
        public MessageService MessageService { get; set; } = null!;

        protected override void OnInitialized()
        {
            MessageService.MessagesStateChanged += HandleMessagesStateChanged;
        }

        private void HandleMessagesStateChanged()
        {
            StateHasChanged();
        }
    }
}
