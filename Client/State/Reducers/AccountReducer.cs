using MoneyManager.Client.State.Actions;
using System.Linq;

namespace MoneyManager.Client.State.Reducers
{
    public class AccountReducer
    {
        public static AccountsState Reduce(AccountsState state, object action)
        {
            var newState = (AccountsState)state.Clone();

            switch (action)
            {
                case AccountActions.AddRange a:
                    newState.Accounts = state.Accounts.Concat(a.Accounts).ToList();
                    return newState;

                case AccountActions.Add a:
                    newState.Accounts = state.Accounts.Concat(new[] { a.Account }).ToList();
                    return newState;

                case AccountActions.Edit a:
                    var newAccounts = state.Accounts.ToList();
                    int accountIndex = state.Accounts.IndexOf(
                        newAccounts.FirstOrDefault(x => x?.Id == a.Account.Id)
                    );
                    newAccounts[accountIndex] = a.Account;

                    newState.Accounts = newAccounts;
                    return newState;

                case AccountActions.SetProperty a:
                    newState.GetType().GetProperty(a.PropertyName).SetValue(newState, a.NewValue);
                    return newState;

                default:
                    return newState;
            }
        }
    }
}
