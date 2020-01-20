using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Actions;
using MoneyManager.Models.Domain;
using MoneyManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MoneyManager.Client.Pages.Categories
{
    public class EditCategoryBase : ComponentBase
    {
        [Parameter]
        public int? CategoryId { get; set; }
        protected bool IsNew => CategoryId == null;

        protected Category FormModel { get; set; } = new Category();

        [Inject]
        protected Store<AppState> Store { get; set; } = null!;
        protected CategoryState CategoryState => Store.State.CategoryState;

        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        public bool IsLoading { get; private set; } = false;
        public bool IsSaving { get; private set; } = false;


        protected override async Task OnInitializedAsync()
        {
            Store.StateChanged += Store_StateChanged;
            if (!IsNew)
            {
                if (CategoryState.IsFirstLoadComplete)
                {
                    var existingCategory = CategoryState.Categories.Where(c => c.Id == CategoryId).FirstOrDefault();
                    if (existingCategory == null)
                    {
                        NavManager.NavigateTo("/categories/create");
                        return;
                    }

                    FormModel = existingCategory;
                }
                else
                {
                    IsLoading = true;
                }
            }
        }

        private void Store_StateChanged(object sender, AppState e)
        {
            if (!IsNew && IsLoading && CategoryState.IsFirstLoadComplete)
            {
                var existingCategory = CategoryState.Categories.Where(c => c.Id == CategoryId).FirstOrDefault();
                if (existingCategory == null)
                {
                    NavManager.NavigateTo("/categories/create");
                    return;
                }

                FormModel = existingCategory;

                IsLoading = false;
            }

            StateHasChanged();
        }

        protected string IsFormValidString(EditContext editContext, Expression<Func<object>> expression)
        {
            return editContext.GetValidationMessages(expression).Count() != 0 ? "error" : "";
        }

        #region Html Events
        protected async Task HandleValidSubmit()
        {
            if (IsSaving)
            {
                return;
            }

            IsSaving = true;
            if (IsNew)
            {
                var createdCategory = await CategoryService.CreateCategoryAsync(FormModel);
                Store.Dispath(new CategoryActions.Add(createdCategory));
                NavManager.NavigateTo("/categories");
            }
            IsSaving = false;
        }
        #endregion
    }
}
