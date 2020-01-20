using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Core.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<bool> Create(User user);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetOne(int id);
    }
}
