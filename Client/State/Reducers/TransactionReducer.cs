using MoneyManager.Client.State.Actions;
using System.Linq;

namespace MoneyManager.Client.State.Reducers
{
    public class TransactionReducer
    {
        public static TransactionState Reduce(TransactionState state, object action)
        {
            var newState = (TransactionState)state.Clone();

            switch (action)
            {
                case TransactionActions.AddRange a:
                    newState.Transactions = state.Transactions.Concat(a.Items).ToList();
                    return newState;

                case TransactionActions.Add a:
                    newState.Transactions = state.Transactions.Concat(new[] { a.Item }).ToList();
                    return newState;

                case TransactionActions.Edit a:
                    var newAccounts = state.Transactions.ToList();
                    int accountIndex = state.Transactions.IndexOf(
                        newAccounts.FirstOrDefault(x => x?.Id == a.Item.Id)
                    );
                    newAccounts[accountIndex] = a.Item;

                    newState.Transactions = newAccounts;
                    return newState;

                case TransactionActions.Delete a:
                    newState.Transactions = state.Transactions.Where(t => t.Id != a.Item.Id).ToList();
                    return newState;

                case TransactionActions.SetProperty a:
                    newState.GetType().GetProperty(a.PropertyName).SetValue(newState, a.NewValue);
                    return newState;

                default:
                    return newState;
            }
        }
    }
}
