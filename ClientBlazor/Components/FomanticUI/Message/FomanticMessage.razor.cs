using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Services;
using System;
using System.Text;

namespace MoneyManager.Client.Components.FomanticUI.Message
{
    public class FomanticMessageBase : ComponentBase
    {
        [Parameter]
        public RenderFragment? Header { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; } = null!;

        [Parameter]
        public EmphasisEnum Emphasis { get; set; } = EmphasisEnum.Normal;

        [Parameter]
        public string? Icon { get; set; }

        protected string MessageClassName
        {
            get
            {
                var className = new StringBuilder("ui message");
                className.Append(" " + EmphasisStr);

                if (!string.IsNullOrEmpty(Icon))
                {
                    className.Append(" icon");
                }

                return className.ToString();
            }
        }


        protected override void OnInitialized()
        {
            //if (ChildContent == null)
            //{
            //    throw new Exception("Fomantic Message mus have content");
            //}
        }



        protected string EmphasisStr =>
            Emphasis switch
            {
                EmphasisEnum.Normal => "",
                EmphasisEnum.Positive => "positive",
                EmphasisEnum.Negative => "negative",
                EmphasisEnum.Success => "success",
                EmphasisEnum.Error => "error",
                EmphasisEnum.Warning => "warning",
                EmphasisEnum.Info => "info",
                _ => ""
            };

        //private void MessageService_OnShow(object modalOptions)
        //{
        //    Options = (ModalOptions)modalOptions;

        //    StateHasChanged();
        //}

        //private void MessageService_OnClose()
        //{
        //    Options = null;
        //    IsActionLoading = false;

        //    StateHasChanged();
        //}

        //private void ModalService_OnSetLoading(bool value)
        //{
        //    IsActionLoading = value;
        //}

        //protected void HandleNoClick()
        //{
        //    Options!.OnNo.Invoke();
        //}

        //protected void HandleYesClick()
        //{
        //    Options!.OnYes.Invoke();
        //}
    }
    public enum EmphasisEnum
    {
        Normal,
        Positive,
        Negative,
        Success,
        Error,
        Warning,
        Info
    }
}
