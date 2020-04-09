using MoneyManager.Models.Domain;

namespace MoneyManager.Client.State.Actions
{
    public static class UserActions
    {
        public class Set
        {
            public User User{ get; set; }

            public Set(User user)
            {
                User = user;
            }
        }
    }
}
