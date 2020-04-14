using Dapper;
using MoneyManager.Core.Services;
using MoneyManager.Models.Domain;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken>
    {
        Task<RefreshToken> GetAsync(string refreshToken);
    }

    public class DapperRefreshTokenRepository : DapperGenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public DapperRefreshTokenRepository(IDbTransaction transaction, CurrentUserService currentUserService)
            : base(transaction, "RefreshToken", currentUserService)
        {
        }

        public async Task<RefreshToken> GetAsync(string token)
        {
            string sql = @$"
                SELECT * FROM ""{TableName}"" as rt
                    JOIN User as user
                        ON rt.UserId = user.Id
                WHERE rt.Token=@token
            ";

            var items = await Connection.QueryAsync<RefreshToken, User, RefreshToken>(
                sql,
                map: (refreshToken, user) =>
                {
                    refreshToken.User = user;
                    return refreshToken;
                },
                param: new { token },
                Transaction
            );
            return items.FirstOrDefault();
        }
    }
}
