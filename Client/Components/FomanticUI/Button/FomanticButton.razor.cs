using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.Components.FomanticUI.Button
{

    public class FomanticButtonBase : ComponentBase
    {
        [Parameter]
        public EventCallback OnClick { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public ButtonHtmlElementEnum HtmlElement { get; set; } = ButtonHtmlElementEnum.Button;

        [Parameter]
        public string Href { get; set; } = string.Empty;

        [Parameter]
        public ButtonHtmlTypeEnum HtmlType { get; set; } = ButtonHtmlTypeEnum.Button;

        [Parameter]
        public ButtonEmphasisEnum Emphasis { get; set; } = ButtonEmphasisEnum.Normal;

        [Parameter]
        public bool IsActive { get; set; } = false;

        [Parameter]
        public bool IsDisabled { get; set; } = false;

        [Parameter]
        public bool IsInverted { get; set; } = false;

        [Parameter]
        public bool IsLoading { get; set; } = false;

        [Parameter]
        public LoadingAnimationEnum LoadingAnimation { get; set; } = LoadingAnimationEnum.Normal;


        protected string ClassName
        {
            get
            {
                var strBuilder = new StringBuilder($"ui button");
                if (Emphasis != ButtonEmphasisEnum.Normal)
                {
                    strBuilder.Append(" ");
                    strBuilder.Append(EmphasisStr);
                }
                if (IsActive)
                {
                    strBuilder.Append(" active");
                }
                if (IsDisabled)
                {
                    strBuilder.Append(" disabled");
                }
                if (IsInverted)
                {
                    strBuilder.Append(" inverted");
                }
                if (IsInverted)
                {
                    strBuilder.Append(" inverted");
                }
                if (IsLoading)
                {
                    strBuilder.Append(" loading");
                    if (LoadingAnimation != LoadingAnimationEnum.Normal)
                    {
                        strBuilder.Append(" ");
                        strBuilder.Append(LoadingAnimationStr);
                    }
                }
                return strBuilder.ToString();
            }
        }

        protected string ButtonHtmlTypeStr =>
            HtmlType switch
            {
                ButtonHtmlTypeEnum.Button => "button",
                ButtonHtmlTypeEnum.Submit => "submit",
                ButtonHtmlTypeEnum.Reset => "reset",
                _ => "button"
            };

        protected string EmphasisStr =>
            Emphasis switch
            {
                ButtonEmphasisEnum.Normal => "",
                ButtonEmphasisEnum.Primary => "primary",
                ButtonEmphasisEnum.Secondary => "secondary",
                ButtonEmphasisEnum.Positive => "positive",
                ButtonEmphasisEnum.Negative => "negative",
                _ => "normal"
            };

        protected string LoadingAnimationStr =>
            LoadingAnimation switch
            {
                LoadingAnimationEnum.Normal => "",
                LoadingAnimationEnum.Double => "double",
                LoadingAnimationEnum.Elastic => "elastic",
                _ => "normal"
            };


        protected async Task HandleClick(MouseEventArgs e)
        {
            await OnClick.InvokeAsync(e);
        }
    }
    public enum ButtonHtmlElementEnum
    {
        Button,
        Anchor
    }

    public enum ButtonHtmlTypeEnum
    {
        Button,
        Submit,
        Reset
    }

    public enum ButtonEmphasisEnum
    {
        Normal,
        Primary,
        Secondary,
        Positive,
        Negative
    }
    public enum LoadingAnimationEnum
    {
        Normal,
        Double,
        Elastic
    }
}
