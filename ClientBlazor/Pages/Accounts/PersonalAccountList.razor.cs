using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Models.Domain;
using MoneyManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Client.Pages.Accounts
{
    public class PersonalAccountListBase : ComponentBase
    {
        [Inject]
        protected Store<AppState> Store { get; set; }

        [Inject]
        protected AccountService AccountService { get; set; }

        protected AccountsState AccountsState => Store.State.AccountsState;

        protected IList<GetPersonalAccountVm> PersonalAccountsVms { get; private set; } = null!;
        protected bool IsLoading { get; private set; } = false;


        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            PersonalAccountsVms = await AccountService.GetAllPersonalAccountsAsync();
            IsLoading = false;
        }

        private void Store_StateChanged(object sender, AppState e)
        {
            Console.WriteLine("State changed");
            StateHasChanged();
        }

        protected async Task Refresh(bool force = false)
        {
            //if (IsLoading && !force)
            //{
            //    return;
            //}

            //IsLoading = true;
            //await AccountService.GetAllAccountsAsync();
            //IsLoading = false;
        }
    }
}
