using MoneyManager.Models.Domain;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
