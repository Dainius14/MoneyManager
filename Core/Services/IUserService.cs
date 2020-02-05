using MoneyManager.Models.Domain;
using MoneyManager.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Core.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task SaveRefreshToken(int userId, string token);
        Task<bool> IsRefreshTokenValid(string refreshToken);
        Task InvalidateRefreshToken(string refreshToken);
        Task<User> Create(RegisterUserVm user);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetOne(int id);
    }
}
