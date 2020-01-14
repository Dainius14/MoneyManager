using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;

namespace MoneyManager.Client.Components.FomanticUI.Message
{
    public sealed class FomanticMessageBuilder
    {
        private RenderFragment _renderFragment;

        public FomanticMessageBuilder(Action<FomanticMessageBuilderDelegate> msgBuilder)
        {
            _renderFragment = new RenderFragment(builder =>
            {
                builder.OpenComponent(1, typeof(FomanticMessage));

                FomanticMessageBuilderDelegate @delegate = new FomanticMessageBuilderDelegate(builder);
                
                msgBuilder.Invoke(@delegate);
                builder.CloseComponent();
            });
        }

        public RenderFragment GetMessage()
        {
            return _renderFragment;
        }
    }

    public sealed class FomanticMessageBuilderDelegate
    {
        private RenderTreeBuilder _builder;

        public FomanticMessageBuilderDelegate(RenderTreeBuilder builder)
        {
            _builder = builder;
        }

        public FomanticMessageBuilderDelegate SetContent(string content)
        {
            _builder.AddAttribute(2, nameof(FomanticMessage.ChildContent), (RenderFragment)((attrBuilder) =>
            {
                attrBuilder.AddContent(3, (MarkupString)content);
            }
            ));
            return this;
        }

        public FomanticMessageBuilderDelegate SetHeader(string header)
        {
            _builder.AddAttribute(4, nameof(FomanticMessage.Header), (RenderFragment)((attrBuilder) =>
            {
                attrBuilder.AddContent(5, (MarkupString)header);
            }
            ));
            return this;
        }

        public FomanticMessageBuilderDelegate SetEmphasis(EmphasisEnum emphasis)
        {
            _builder.AddAttribute(6, nameof(FomanticMessage.Emphasis), emphasis);
            return this;
        }

        public FomanticMessageBuilderDelegate SetIcon(string icon)
        {
            _builder.AddAttribute(7, nameof(FomanticMessage.Icon), icon);
            return this;
        }
    }
}
