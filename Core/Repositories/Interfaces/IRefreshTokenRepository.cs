using MoneyManager.Models.Domain;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetAsync(string refreshToken);
    }
}
