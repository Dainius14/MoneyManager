using MoneyManager.Client.State.Actions;
using System.Linq;

namespace MoneyManager.Client.State.Reducers
{
    public class CategoryReducer
    {
        public static CategoryState Reduce(CategoryState state, object action)
        {
            var newState = (CategoryState)state.Clone();

            switch (action)
            {
                case CategoryActions.AddRange a:
                    newState.Categories = state.Categories.Concat(a.Items).ToList();
                    return newState;

                case CategoryActions.Add a:
                    newState.Categories = state.Categories.Concat(new[] { a.Item }).ToList();
                    return newState;

                case CategoryActions.Edit a:
                    var newCategories = state.Categories.ToList();
                    int accountIndex = state.Categories.IndexOf(
                        newCategories.FirstOrDefault(x => x?.Id == a.Item.Id)
                    );
                    newCategories[accountIndex] = a.Item;

                    newState.Categories = newCategories;
                    return newState;

                case CategoryActions.SetProperty a:
                    newState.GetType().GetProperty(a.PropertyName).SetValue(newState, a.NewValue);
                    return newState;

                default:
                    return newState;
            }
        }
    }
}
