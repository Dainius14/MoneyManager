using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Client.Components.FomanticUI.Dropdown
{
    public class FomanticDropdownBase<TItem> : ComponentBase
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public RenderFragment<TItem> SelectedItemTemplate { get; set; } = null!;
        [Parameter]
        public RenderFragment<TItem> ResultItemTemplate { get; set; } = null!;

        [Parameter]
        public Func<string?, Task<IEnumerable<TItem>>> SearchFunction { get; set; } = null!;

        [Parameter]
        public string PlaceholderText { get; set; } = string.Empty;

        [Parameter]
        public bool IsFluid { get; set; } = true;


        [Parameter]
        public bool IsClearable { get; set; } = false;
        public bool CanClearInput => IsClearable && (SelectedItem != null || !string.IsNullOrEmpty(SearchText));

        protected bool ShowResults = false;

        protected IList<TItem> Suggestions = new List<TItem>();

        [Parameter]
        public IList<TItem>? GivenItems { get; set; }

        [Parameter]
        public TItem SelectedItem { get; set; }  // TODO add type constraints when possible
        [Parameter]
        public EventCallback<TItem> SelectedItemChanged { get; set; }


        protected int? SelectedItemIndex;
        protected string SearchText { get; set; } = string.Empty;

        protected IList<TItem> Items => IsSearchable ? Suggestions : GivenItems;
        public bool IsSearchable => SearchFunction != null;

        protected ElementReference InputElementRef { get; set; }
        protected ElementReference ComponentElementRef { get; set; }

        protected string SearchBoxTextClass
        {
            get
            {
                string className = "text";
                if (SelectedItem == null)
                {
                    className += " default";
                }
                if (!string.IsNullOrEmpty(SearchText))
                {
                    className += " filtered";
                }
                return className;
            }
        }

        protected string ComponentClass
        {
            get
            {
                var className = new StringBuilder("ui selection dropdown");

                if (ShowResults)
                {
                    className.Append(" active visible");
                }
                if (IsClearable)
                {
                    className.Append(" clearable");
                }
                if (IsSearchable)
                {
                    className.Append(" search");
                }
                if (IsFluid)
                {

                    className.Append(" fluid");
                }
                return className.ToString();
            }
        }

        protected override void OnInitialized()
        {
            CheckIfAllParamsAreGiven();

            if (ResultItemTemplate == null)
            {
                ResultItemTemplate = SelectedItemTemplate;
            }

            if (IsSearchable)
            {
                GetNewSuggestions();
            }
            else
            {
                Suggestions = GivenItems!;

                SelectedItemIndex = Suggestions.IndexOf(SelectedItem);
            }
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JSRuntime.AddOnClickOutsideListener(ComponentElementRef, DotNetObjectReference.Create(this), nameof(HandleClickOutside));

                if (IsSearchable)
                {
                    await JSRuntime.PreventInputKeyDownDefaults(InputElementRef!);
                }
            }
        }

        protected void ToggleVisibility()
        {
            ShowResults = !ShowResults;
        }
        protected void OnSearchBoxClick()
        {
            ShowResults = true;
            GetNewSuggestions();
        }

        protected void OnSearchInputFocus()
        {
            ShowResults = true;
            GetNewSuggestions();
        }

        protected async Task OnSuggestionClick(TItem item, int index)
        {
            SearchText = String.Empty;
            ShowResults = false;
            await SetSelectedItem(item, index);
            if (IsSearchable)
            {
                await JSRuntime.BlurElement(InputElementRef);
            }
        }

        protected async Task OnSearchInput(ChangeEventArgs e)
        {
            SearchText = (string)e.Value;
            GetNewSuggestions();
        }


        protected void OnSearchInputBlur()
        {
            BlurComponent();
        }

        private void BlurComponent()
        {
            SearchText = String.Empty;
            ShowResults = false;
            StateHasChanged();
        }

        private async Task SelectPreviousItem()
        {
            if (Suggestions.Count == 0)
            {
                return;
            }
            int newIndex;
            if (SelectedItemIndex == null)
            {
                newIndex = Suggestions.Count - 1;
            }
            else
            {
                newIndex = (int)SelectedItemIndex - 1;
                if (newIndex == -1)
                {
                    newIndex = Suggestions.Count - 1;
                }
            }
            await SetSelectedItem(Suggestions[newIndex], newIndex);
        }
        private async Task SelectNextItem()
        {
            if (Suggestions.Count == 0)
            {
                return;
            }
            int newIndex;
            if (SelectedItemIndex == null)
            {
                newIndex = 0;
            }
            else
            {
                newIndex = (int)SelectedItemIndex + 1;
                if (newIndex == Suggestions.Count)
                {
                    newIndex = 0;
                }
            }
            await SetSelectedItem(Suggestions[newIndex], newIndex);
        }

        private async void GetNewSuggestions()
        {
            if (IsSearchable)
            {
                Suggestions = (await SearchFunction.Invoke(SearchText)).ToList();
            }
        }

        private async Task SetSelectedItem(TItem item, int index)
        {
            SelectedItem = item;
            SelectedItemIndex = index;
            await SelectedItemChanged.InvokeAsync(item);
        }

        private async Task ClearSelectedItem()
        {
            SelectedItem = default;
            SelectedItemIndex = null;
            await SelectedItemChanged.InvokeAsync(default);
        }

        #region Component HTML events
        protected async Task OnSearchInputKeyUp(KeyboardEventArgs e)
        {
            Console.WriteLine("OnSearchInputKeyUp: " + e.Key);
            switch (e.Key)
            {
                case "ArrowUp":
                    await SelectPreviousItem();
                    break;
                case "ArrowDown":
                    await SelectNextItem();
                    break;
                case "Enter":
                    SearchText = String.Empty;
                    ShowResults = false;
                    await JSRuntime.BlurElement(InputElementRef);
                    break;
            }
        }

        protected async Task OnClearButtonClick()
        {
            Console.WriteLine("click");
            await ClearSelectedItem();
            GetNewSuggestions();
        }
        #endregion

        private void CheckIfAllParamsAreGiven()
        {
            if (SearchFunction == null && GivenItems == null)
            {
                ThrowMissingParamException(nameof(GivenItems), nameof(SearchFunction) + " is null");
            }

            if (SelectedItemTemplate == null)
            {
                ThrowMissingParamException(nameof(SelectedItemTemplate));
            }
        }

        private void ThrowMissingParamException(string requiredParam, string? ifClause = null)
        {
            string text = $"{GetType()}: {requiredParam} is required{(ifClause != null ? " if " + ifClause : "")}";
            throw new NullReferenceException(text);
        }

        [JSInvokable]
        public void HandleClickOutside()
        {
            Console.WriteLine("handling");
            BlurComponent();
        }
    }
}
