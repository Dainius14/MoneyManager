using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MoneyManager.Client.Components.FomanticUI.Button;
using MoneyManager.Client.Components.FomanticUI.Message;
using MoneyManager.Client.Components.FomanticUI.Modal;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Actions;
using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Timers;

namespace MoneyManager.Client.Pages.Transactions
{
    public class TransactionListBase : ComponentBase
    {
        [Inject]
        protected ModalService ModalService { get; set; } = null!;

        [Inject]
        protected MessageService MessageService { get; set; } = null!;

        [Inject]
        protected NavigationManager NavManager { get; set; } = null!;

        [Inject]
        protected Store<AppState> Store { get; set; } = null!;

        protected TransactionState TransactionState => Store.State.TransactionState;

        protected IList<Transaction> Transactions => TransactionState.Transactions;

        protected bool IsLoading => TransactionState.IsLoading || !TransactionState.IsFirstLoadComplete;


        private bool _isDeletingTransaction = false;

        protected override void OnInitialized()
        {
            Store.StateChanged += Store_StateChanged;
        }

        private void Store_StateChanged(object sender, AppState e)
        {
            StateHasChanged();
        }

        protected void HandleRowDelete(Transaction transaction)
        {
            var options = new ModalOptions(
                "Deleting transaction",
                $"Do you really want to delete the transaction <b>{SecurityElement.Escape(transaction.Description)}</b>?",
                async () => await OnConfirmDelete(transaction),
                OnCancelDelete
                );
            ModalService.Show(options);
        }
        protected async Task OnConfirmDelete(Transaction transaction)
        {
            if (_isDeletingTransaction)
            {
                return;
            }

            _isDeletingTransaction = true;
            ModalService.SetLoading(true);
            bool success = await TransactionService.DeleteTransactionAsync((int)transaction.ID!);

            if (success)
            {
                var transactionDeletedMsg = new FomanticMessageBuilder(builder => builder
                    .SetHeader("Transaction deleted")
                    .SetContent($"Transaction <b>{SecurityElement.Escape(transaction.Description)}</b> has been successfully deleted")
                    .SetEmphasis(EmphasisEnum.Success)
                    .SetIcon("check")
                ).GetMessage();
                MessageService.Show(transactionDeletedMsg);
                Store.Dispath(new TransactionActions.Delete(transaction));
            }
            ModalService.Close();
            _isDeletingTransaction = false;
        }
        protected void OnCancelDelete()
        {
            ModalService.Close();
        }

        protected string GetTransactionAmountColor(Transaction transaction) =>
            transaction.TransactionType switch
            {
                TransactionTypeEnum.Expense => "red",
                TransactionTypeEnum.Income => "green",
                _ => ""
            };
        protected string GetTransactionAmountText(Transaction transaction)
        {
            string currencySymbol = transaction.TransactionDetails.FirstOrDefault()?.Currency.Symbol ?? "";
            return transaction.TransactionType switch
            {
                TransactionTypeEnum.Expense => $"-{transaction.AmountStr} {currencySymbol}",
                TransactionTypeEnum.Income => $"+{transaction.AmountStr} {currencySymbol}",
                _ => $"{transaction.AmountStr} {currencySymbol}"
            };
        }


    }
}
