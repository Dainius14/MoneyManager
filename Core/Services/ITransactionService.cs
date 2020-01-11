using MoneyManager.Core.Services.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;

namespace MoneyManager.Core.Services
{
    public interface ITransactionService
    {
        Task<Response<IEnumerable<Transaction>>> ListAsync();
        Task<Response<Transaction>> CreateAsync(Transaction transaction);
        Task<Response<Transaction>> UpdateAsync(int id, Transaction transaction);
        Task<Response<Transaction>> DeleteAsync(int id);
    }
}
