namespace MoneyManager.Client.State.Reducers
{
    public static class RootReducer
    {
        public static AppState Reducer(AppState state, object action)
        {
            return new AppState
            {
                AccountsState = AccountReducer.Reduce(state.AccountsState, action),
                CategoryState = CategoryReducer.Reduce(state.CategoryState, action),
                CurrencyState = CurrencyReducer.Reduce(state.CurrencyState, action),
                TransactionState = TransactionReducer.Reduce(state.TransactionState, action)
            };
        }
    }
}
