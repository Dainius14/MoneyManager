using MoneyManager.Models.Domain;
using System.Collections.Generic;

namespace MoneyManager.Client.State.Actions
{
    public static class AccountActions
    {
        public class Add
        {
            public Account Account { get; set; } = null!;

            public Add(Account a)
            {
                Account = a;
            }
        }

        public class AddRange
        {
            public IEnumerable<Account> Accounts { get; set; }

            public AddRange(IEnumerable<Account> a)
            {
                Accounts = a;
            }
        }

        public class Edit
        {
            public Account Account { get; set; }
            
            public Edit(Account a)
            {
                Account = a;
            }
        }

        public class SetProperty
        {
            public string PropertyName { get; private set; }
            public object NewValue { get; private set; }
            public SetProperty(string propertyName, object value)
            {
                PropertyName = propertyName;
                NewValue = value;
            }
        }

        public class SetCurrentBalance
        {
            public int AccountId { get; private set; }
            public double Balance { get; private set; }
            public SetCurrentBalance(int accountId, double balance)
            {
                AccountId = accountId;
                Balance = balance;
            }

        }
    }
}
