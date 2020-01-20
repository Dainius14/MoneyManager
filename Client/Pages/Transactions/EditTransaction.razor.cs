using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using MoneyManager.Client.Components.FomanticUI.Message;
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
using System.Security;
using System.Threading.Tasks;

namespace MoneyManager.Client.Pages.Transactions
{
    public class EditTransactionBase : ComponentBase
    {
        [Inject]
        protected MessageService MessageService { get; set; } = null!;

        [Parameter]
        public int? TransactionId { get; set; }
        protected bool IsNew => TransactionId == null;

        protected Transaction FormModel { get; set; } = new Transaction();

        [Inject]
        protected Store<AppState> Store { get; set; } = null!;
        protected TransactionState TransactionState => Store.State.TransactionState;

        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        public bool IsLoading { get; private set; } = false;
        public bool IsSaving { get; private set; } = false;


        protected override void OnInitialized()
        {
            Store.StateChanged += Store_StateChanged;
            NavManager.LocationChanged += NavManager_LocationChanged;

            if (!IsNew)
            {
                if (TransactionState.IsFirstLoadComplete)
                {
                    var existingTransaction = TransactionState.Transactions.Where(c => c.Id == TransactionId).FirstOrDefault();
                    if (existingTransaction == null)
                    {
                        NavManager.NavigateTo("/transactions/create");
                        return;
                    }

                    FormModel = existingTransaction;
                }
                else
                {
                    IsLoading = true;
                }
            }
        }

        private void Init()
        {
            IsSaving = false;
            IsLoading = false;
        }

        private void NavManager_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            Init();
        }

        private void Store_StateChanged(object sender, AppState e)
        {
            if (!IsNew && IsLoading && TransactionState.IsFirstLoadComplete)
            {
                var existingCategory = TransactionState.Transactions.Where(c => c.Id == TransactionId).FirstOrDefault();
                if (existingCategory == null)
                {
                    NavManager.NavigateTo("/transactions/create");
                    return;
                }

                FormModel = existingCategory;

                IsLoading = false;
            }

            StateHasChanged();
        }

        protected void AddDetailsRow()
        {
            FormModel.TransactionDetails.Add(new TransactionDetails
            { 
                FromAccount = FormModel.TransactionDetails.First().FromAccount,
                ToAccount = null,
                Currency = FormModel.TransactionDetails.First().Currency
            });
        }

        protected string IsFormValidString(EditContext editContext, Expression<Func<object>> expression)
        {
            return editContext.GetValidationMessages(expression).Count() != 0 ? "error" : "";
        }

        protected async Task<IEnumerable<Account>> SuggestFromAccount(string searchText)
        {
            if (FormModel.TransactionType == TransactionTypeEnum.Expense
                || FormModel.TransactionType == TransactionTypeEnum.Transfer)
            {
                return Store.State.AccountsState.Accounts
                    .Where(a => a.IsPersonal && a.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(a => a.Name);
            }
            else if (FormModel.TransactionType == TransactionTypeEnum.Income)
            {
                return Store.State.AccountsState.Accounts
                    .Where(a => !a.IsPersonal && a.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(a => a.Name);
            }
            else
            {
                return Store.State.AccountsState.Accounts
                    .Where(a => a.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(a => a.IsPersonal)
                    .ThenBy(a => a.Name);
            }
        }

        protected async Task<IEnumerable<Account>> SuggestToAccount(string searchText)
        {
            return Store.State.AccountsState.Accounts
                .Where(a => a.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .OrderBy(a => a.IsPersonal)
                .ThenBy(a => a.Name);
        }

        protected async Task<IEnumerable<Category>> SuggestCategory(string searchText)
        {
            return Store.State.CategoryState.Categories
                .Where(x => x.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Name);
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
                var created = await TransactionService.CreateTransactionAsync(FormModel);
                Store.Dispath(new TransactionActions.Add(created));
                NavManager.NavigateTo("/transactions");

                var transactionCreatedMsg = new FomanticMessageBuilder(builder => builder
                    .SetHeader("Transaction created")
                    .SetContent($"Transaction <b><a href=\"/transactions/{created.Id}\">{SecurityElement.Escape(created.Description)}</a></b> has been successfully created")
                    .SetEmphasis(EmphasisEnum.Success)
                    .SetIcon("check")
                ).GetMessage();
                MessageService.Show(transactionCreatedMsg);
            }
            IsSaving = false;
        }
        #endregion
    }
}
