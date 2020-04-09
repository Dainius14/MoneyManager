using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Client.Pages.Accounts
{
    public class NonPersonalAccountListBase : ComponentBase
    {
        [Inject]
        protected Store<AppState> Store { get; set; }

        [Inject]
        protected AccountService AccountService { get; set; }


        protected AccountsState AccountsState => Store.State.AccountsState;

        protected IList<GetNonPersonalAccountVm> NonPersonalAccountsVms { get; private set; } = null!;
        protected bool IsLoading { get; private set; } = false;


        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            NonPersonalAccountsVms = await AccountService.GetAllNonPersonalAccountsAsync() ?? new List<GetNonPersonalAccountVm>();
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
