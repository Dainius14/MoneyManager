using Microsoft.AspNetCore.Components;
using MoneyManager.Client.Services;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Actions;
using System;
using System.Threading.Tasks;

namespace MoneyManager.Client
{
    public class Bootstrapper
    {
        private readonly Store<AppState> _store;
        private readonly CategoryService _categoryService;
        private readonly CurrencyService _currencyService;
        private readonly TransactionService _transactionService;

        private bool _gotData = false;

        public Bootstrapper(Store<AppState> store, TransactionService transactionService,
            CurrencyService currencyService, CategoryService categoryService)
        {
            _store = store;
            _transactionService = transactionService;
            _currencyService = currencyService;
            _categoryService = categoryService;
        }

        public void GetData()
        {
            if (!_gotData)
            {
                Parallel.Invoke(
                      //async () => await GetCurrenciesAsync(),
                      //async () => await GetAccountsAsync(),
                      //async () => await GetCategoriesAsync(),
                      async () => await GetTransactionsAsync()
                  );
            }
        }

        //private async Task GetAccountsAsync()
        //{
        //    _store.Dispath(new AccountActions.SetProperty(nameof(AccountsState.IsLoading), true));
        //    var items = await AccountService.GetAllAccountsAsync();
        //    _store.Dispath(new AccountActions.AddRange(items));
        //    _store.Dispath(new AccountActions.SetProperty(nameof(AccountsState.IsFirstLoadComplete), true));
        //    _store.Dispath(new AccountActions.SetProperty(nameof(AccountsState.IsLoading), false));
        //}

        private async Task GetCurrenciesAsync()
        {
            _store.Dispath(new CurrencyActions.SetProperty(nameof(CurrencyState.IsLoading), true));
            var items = await _currencyService.GetAllCurrenciesAsync();
            _store.Dispath(new CurrencyActions.AddRange(items));
            _store.Dispath(new CurrencyActions.SetProperty(nameof(CurrencyState.IsFirstLoadComplete), true));
            _store.Dispath(new CurrencyActions.SetProperty(nameof(CurrencyState.IsLoading), false));
        }

        private async Task GetCategoriesAsync()
        {
            _store.Dispath(new CategoryActions.SetProperty(nameof(CategoryState.IsLoading), true));
            var items = await _categoryService.GetAllCategoriesAsync();
            _store.Dispath(new CategoryActions.AddRange(items));
            _store.Dispath(new CategoryActions.SetProperty(nameof(CategoryState.IsFirstLoadComplete), true));
            _store.Dispath(new CategoryActions.SetProperty(nameof(CategoryState.IsLoading), false));
        }

        private async Task GetTransactionsAsync()
        {
            _store.Dispath(new TransactionActions.SetProperty(nameof(TransactionState.IsLoading), true));
            var items = await _transactionService.GetAllTransactionsAsync();
            _store.Dispath(new TransactionActions.AddRange(items));
            _store.Dispath(new TransactionActions.SetProperty(nameof(TransactionState.IsFirstLoadComplete), true));
            _store.Dispath(new TransactionActions.SetProperty(nameof(TransactionState.IsLoading), false));
        }
    }
}
