using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Services;
using System.Text;

namespace MoneyManager.Client.Components.FomanticUI.Modal
{
    public class FomanticModalBase : ComponentBase
    {
        [Inject]
        protected ModalService ModalService { get; set; } = null!;

        protected string DimmerClassName
        {
            get
            {
                var className = new StringBuilder("ui dimmer");

                if (IsVisible)
                {
                    className.Append(" visible active");
                }
                else
                {
                    className.Append(" hidden");
                }

                return className.ToString();
            }
        }

        protected ModalOptions? Options { get; private set; }

        protected bool IsActionLoading { get; private set; } = false;

        protected bool IsVisible => Options != null;

        protected override void OnInitialized()
        {
            ModalService.OnShow += ModalService_OnShow;
            ModalService.OnClose += ModalService_OnClose;
            ModalService.OnSetLoading += ModalService_OnSetLoading;
        }

        private void ModalService_OnShow(object modalOptions)
        {
            Options = (ModalOptions)modalOptions;

            StateHasChanged();
        }

        private void ModalService_OnClose()
        {
            Options = null;
            IsActionLoading = false;

            StateHasChanged();
        }

        private void ModalService_OnSetLoading(bool value)
        {
            IsActionLoading = value;
        }

        protected void HandleNoClick()
        {
            Options!.OnNo.Invoke();
        }

        protected void HandleYesClick()
        {
            Options!.OnYes.Invoke();
        }
    }
}
