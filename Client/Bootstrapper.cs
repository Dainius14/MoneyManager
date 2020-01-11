using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Actions;
using System;
using System.Threading.Tasks;

namespace MoneyManager.Client
{
    public class Bootstrapper
    {
        private Store<AppState> _store;

        public Bootstrapper(Store<AppState> store)
        {
            _store = store;
        }

        public async Task InitAsync()
        {
            Parallel.Invoke(
                async () => await GetCurrenciesAsync(),
                async () => await GetAccountsAsync(),
                async () => await GetCategoriesAsync(),
                async () => await GetTransactionsAsync()
            );
        }

        private async Task GetAccountsAsync()
        {
            _store.Dispath(new AccountActions.SetProperty(nameof(AccountsState.IsLoading), true));
            var items = await RESTAccountService.GetAllAccountsAsync();
            _store.Dispath(new AccountActions.AddRange(items));
            _store.Dispath(new AccountActions.SetProperty(nameof(AccountsState.IsFirstLoadComplete), true));
            _store.Dispath(new AccountActions.SetProperty(nameof(AccountsState.IsLoading), false));
        }
        private async Task GetCurrenciesAsync()
        {
            _store.Dispath(new CurrencyActions.SetProperty(nameof(CurrencyState.IsLoading), true));
            var items = await RESTCurrencyService.GetAllCurrenciesAsync();
            _store.Dispath(new CurrencyActions.AddRange(items));
            _store.Dispath(new CurrencyActions.SetProperty(nameof(CurrencyState.IsFirstLoadComplete), true));
            _store.Dispath(new CurrencyActions.SetProperty(nameof(CurrencyState.IsLoading), false));
        }

        private async Task GetCategoriesAsync()
        {
            _store.Dispath(new CategoryActions.SetProperty(nameof(CategoryState.IsLoading), true));
            var items = await CategoryService.GetAllCategoriesAsync();
            _store.Dispath(new CategoryActions.AddRange(items));
            _store.Dispath(new CategoryActions.SetProperty(nameof(CategoryState.IsFirstLoadComplete), true));
            _store.Dispath(new CategoryActions.SetProperty(nameof(CategoryState.IsLoading), false));
        }

        private async Task GetTransactionsAsync()
        {
            _store.Dispath(new TransactionActions.SetProperty(nameof(TransactionState.IsLoading), true));
            var items = await TransactionService.GetAllTransactionsAsync();
            _store.Dispath(new TransactionActions.AddRange(items));
            _store.Dispath(new TransactionActions.SetProperty(nameof(TransactionState.IsFirstLoadComplete), true));
            _store.Dispath(new TransactionActions.SetProperty(nameof(TransactionState.IsLoading), false));
        }
    }
}
