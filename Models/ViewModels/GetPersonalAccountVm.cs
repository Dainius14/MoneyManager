using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;

namespace MoneyManager.Models.ViewModels
{
    public class GetPersonalAccountVm
    {
        public Account Account { get; set; }

        public double CurrentBalance { get; set; }

        public string CurrentBalanceStr => string.Format("{0:0,0.00}", CurrentBalance);

        public Currency Currency { get; set; }


        public GetPersonalAccountVm(Account account, double currentBalance, Currency currency)
        {
            Account = account;
            CurrentBalance = currentBalance;
            Currency = currency;
        }
    }
    public class GetPersonalAccountVmDto
    {
        public GetAccountDTO Account { get; set; } = null!;

        public double CurrentBalance { get; set; }

        public GetCurrencyDTO Currency { get; set; } = null!;

        public GetPersonalAccountVmDto()
        {
        }

        public GetPersonalAccountVmDto(GetAccountDTO account, double currentBalance, GetCurrencyDTO currency)
        {
            Account = account;
            CurrentBalance = currentBalance;
            Currency = currency;
        }
    }
}
