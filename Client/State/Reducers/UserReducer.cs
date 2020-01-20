using MoneyManager.Client.State.Actions;

namespace MoneyManager.Client.State.Reducers
{
    public class UserReducer
    {
        public static UserState Reduce(UserState state, object action)
        {
            var newState = (UserState)state.Clone();

            switch (action)
            {
                case UserActions.Set a:
                    newState.User = a.User;
                    return newState;

                default:
                    return newState;
            }
        }
    }
}
