using Dapper;
using MoneyManager.Models.Domain;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public class DapperRefreshTokenRepository : DapperGenericRepository<RefreshToken>, IRefreshTokenRepository
    {
        public DapperRefreshTokenRepository(IDbTransaction transaction)
            : base(transaction, "RefreshToken")
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
