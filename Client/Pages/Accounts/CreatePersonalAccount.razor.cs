﻿using Microsoft.AspNetCore.Components;
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
        #region Injections
        [Inject]
        protected Store<AppState> Store { get; set; } = null!;

        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        [Inject]
        protected AccountService AccountService { get; set; } = null!;
        #endregion

        [Parameter]
        public int? AccountId { get; set; }
        protected bool IsNew => AccountId == null;

        protected EditPersonalAccountVm FormModel { get; set; } = new EditPersonalAccountVm();

        protected IList<Currency> Currencies => Store.State.CurrencyState.Currencies;


        public bool IsSavingAccount { get; private set; } = false;


        protected override async Task OnInitializedAsync()
        {
            Store.StateChanged += Store_StateChanged;

            if (!IsNew)
            {
                var viewModels = await AccountService.GetAllPersonalAccountsAsync();
                var existingVm = viewModels.Where(vm => vm.Account.Id == AccountId).FirstOrDefault();
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
            var savedVm = await AccountService.CreatePersonalAccountAsync(FormModel);
            Store.Dispath(new AccountActions.Add(savedVm.Account));
            IsSavingAccount = false;
        }
        #endregion
    }
}
