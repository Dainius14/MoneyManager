using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.ViewModels
{
    public class GetNonPersonalAccountVm
    {
        public Account Account { get; set; }

        public double CurrentBalance { get; set; }

        public string CurrentBalanceStr => string.Format("{0:0,0.00}", CurrentBalance);


        public GetNonPersonalAccountVm(Account account, double currentBalance)
        {
            Account = account;
            CurrentBalance = currentBalance;
        }
    }
    public class GetNonPersonalAccountVmDto
    {
        public GetAccountDTO Account { get; set; } = null!;

        public double CurrentBalance { get; set; }

        public GetNonPersonalAccountVmDto()
        {
        }

        public GetNonPersonalAccountVmDto(GetAccountDTO account, double currentBalance)
        {
            Account = account;
            CurrentBalance = currentBalance;
        }
    }
}
