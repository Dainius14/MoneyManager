using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Actions;
using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Client.Pages.Transactions
{
    public class TransactionListBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        [Inject]
        protected Store<AppState> Store { get; set; } = null!;

        protected TransactionState TransactionState => Store.State.TransactionState;

        protected IList<Transaction> Transactions => TransactionState.Transactions;

        protected bool IsLoading => TransactionState.IsLoading || !TransactionState.IsFirstLoadComplete;


        protected override void OnInitialized()
        {
            Store.StateChanged += Store_StateChanged;
        }

        private void Store_StateChanged(object sender, AppState e)
        {
            StateHasChanged();
        }

        protected async Task HandleRowDelete(Transaction transaction)
        { 
            bool success = await TransactionService.DeleteTransactionAsync((int)transaction.ID!);
            if (success)
            {
                Store.Dispath(new TransactionActions.Delete(transaction));
            }
        }

    }
}
