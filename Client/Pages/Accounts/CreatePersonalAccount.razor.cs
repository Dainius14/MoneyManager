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

namespace MoneyManager.Client.Pages.Accounts
{
    public class CreatePersonalAccountBase : ComponentBase
    {
        [Parameter]
        public int? AccountId { get; set; }
        protected bool IsNew => AccountId == null;

        protected EditPersonalAccountVm FormModel { get; set; } = new EditPersonalAccountVm();

        [Inject]
        protected Store<AppState> Store { get; set; } = null!;

        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        protected IList<Currency> Currencies => Store.State.CurrencyState.Currencies;


        public bool IsSavingAccount { get; private set; } = false;


        protected override async Task OnInitializedAsync()
        {
            Store.StateChanged += Store_StateChanged;

            if (!IsNew)
            {
                var viewModels = await RESTAccountService.GetAllPersonalAccountsAsync();
                var existingVm = viewModels.Where(vm => vm.Account.ID == AccountId).FirstOrDefault();
                if (existingVm == null)
                {
                    NavManager.NavigateTo("/accounts/personal/create");
                    return;
                }

                FormModel = new EditPersonalAccountVm
                {
                    Name = existingVm.Account.Name,
                    Currency = existingVm.Currency
                };
            }
        }

        private void Store_StateChanged(object sender, AppState e)
        {
            StateHasChanged();
        }

        protected string IsFormValidString(EditContext editContext, Expression<Func<object>> expression)
        {
            return editContext.GetValidationMessages(expression).Count() != 0 ? "error" : "";
        }

        #region Html Events
        protected async Task HandleValidSubmit()
        {
            if (IsSavingAccount)
            {
                return;
            }

            IsSavingAccount = true;
            var savedVm = await RESTAccountService.CreatePersonalAccountAsync(FormModel);
            Store.Dispath(new AccountActions.Add(savedVm.Account));
            IsSavingAccount = false;
        }
        #endregion
    }
}
