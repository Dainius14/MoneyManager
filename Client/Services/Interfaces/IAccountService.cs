using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account> GetAccountAsync(int accountId);
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> EditAccountAsync(Account account);
        Task<bool> DeleteAccountAsync(int accountId);
    }
}
