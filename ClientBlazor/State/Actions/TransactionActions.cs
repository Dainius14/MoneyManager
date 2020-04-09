using MoneyManager.Models.Domain;
using System.Collections.Generic;

namespace MoneyManager.Client.State.Actions
{
    public static class TransactionActions
    {
        public class Add
        {
            public Transaction Item { get; set; } = null!;

            public Add(Transaction item)
            {
                Item = item;
            }
        }

        public class AddRange
        {
            public IEnumerable<Transaction> Items { get; set; }

            public AddRange(IEnumerable<Transaction> item)
            {
                Items = item;
            }
        }

        public class Edit
        {
            public Transaction Item { get; set; }

            public Edit(Transaction item)
            {
                Item = item;
            }
        }

        public class Delete
        {
            public Transaction Item { get; set; }

            public Delete(Transaction item)
            {
                Item = item;
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
    }
}
