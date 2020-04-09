using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace MoneyManager.Client.Components.FomanticUI.Dropdown
{
    public static class JSInterop
    {
        internal static async Task PreventInputKeyDownDefaults(this IJSRuntime jsRuntime, ElementReference element)
        {
            await jsRuntime.InvokeVoidAsync("FomanticUIDropdown.preventInputKeyDownDefaults", element);
        }

        internal static async Task BlurElement(this IJSRuntime jsRuntime, ElementReference element)
        {
            await jsRuntime.InvokeVoidAsync("FomanticUIDropdown.blurElement", element);
        }

        internal static async Task AddOnClickOutsideListener(this IJSRuntime jsRuntime, ElementReference element,
            object objectReference, string methodName)
        {
            await jsRuntime.InvokeVoidAsync("clickOutsideHandler.addListener", element.Id, element,
                objectReference, methodName);
        }
    }
}
