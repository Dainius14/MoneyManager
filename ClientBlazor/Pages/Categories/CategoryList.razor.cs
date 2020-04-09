using Microsoft.AspNetCore.Components;
using MoneyManager.Client.State;
using MoneyManager.Models.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Client.Pages.Categories
{
    public class CategoryListBase : ComponentBase
    {
        [Inject]
        protected Store<AppState> Store { get; set; } = null!;

        protected CategoryState CategoryState => Store.State.CategoryState;

        protected IList<Category> Categories => CategoryState.Categories;
        protected bool IsLoading { get; private set; } = false;


        protected override async Task OnInitializedAsync()
        {
            Store.StateChanged += Store_StateChanged;

            if (CategoryState.IsLoading || !CategoryState.IsFirstLoadComplete)
            {
                IsLoading = true;
            }
        }

        private void Store_StateChanged(object sender, AppState e)
        {
            IsLoading = CategoryState.IsLoading;

            StateHasChanged();
        }
    }
}
