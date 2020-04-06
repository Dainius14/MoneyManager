using MoneyManager.Models.Domain;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<Account> GetByNameAsync(int userId, string name, bool isPersonal);
    }
}
