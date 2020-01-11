using MoneyManager.Core.Services.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;
using MoneyManager.Models.ViewModels;

namespace MoneyManager.Core.Services
{
    public interface IAccountService
    {
        Task<IEnumerable<Account>> ListAsync();
        Task<IEnumerable<GetPersonalAccountVm>> ListPersonalAsync();
        Task<IEnumerable<GetNonPersonalAccountVm>> ListNonPersonalAsync();
        Task<Response<Account>> GetById(int id);
        Task<Response<Account>> CreateAsync(Account account);
        Task<Response<GetPersonalAccountVm>> CreateAsync(EditPersonalAccountVm vm);
        Task<Response<GetNonPersonalAccountVm>> CreateAsync(EditNonPersonalAccountVm vm);
        Task<Response<Account>> UpdateAsync(int id, Account account);
        Task<Response<Account>> DeleteAsync(int id);
    }
}
