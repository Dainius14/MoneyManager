using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.ViewModels
{
    public class AccountVm : Account
    {
        public double CurrentBalance { get; set; }

        public AccountVm(Account account)
            : base(account)
        {

        }
    }
}
