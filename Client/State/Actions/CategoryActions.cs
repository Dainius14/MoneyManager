using MoneyManager.Models.Domain;
using System.Collections.Generic;

namespace MoneyManager.Client.State.Actions
{
    public static class CategoryActions
    {
        public class Add
        {
            public Category Item { get; set; } = null!;

            public Add(Category item)
            {
                Item = item;
            }
        }

        public class AddRange
        {
            public IEnumerable<Category> Items { get; set; }

            public AddRange(IEnumerable<Category> items)
            {
                Items = items;
            }
        }

        public class Edit
        {
            public Category Item { get; set; }
            
            public Edit(Category item)
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
