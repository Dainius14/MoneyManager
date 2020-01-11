using MoneyManager.Models.Domain;
using System.Collections.Generic;

namespace MoneyManager.Client.State.Actions
{
    public static class CurrencyActions
    {
        public class AddRange
        {
            public IEnumerable<Currency> Currencies { get; private set; }

            public AddRange(IEnumerable<Currency> c)
            {
                Currencies = c;
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
