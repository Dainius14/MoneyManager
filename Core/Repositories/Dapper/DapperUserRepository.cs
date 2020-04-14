using Dapper;
using MoneyManager.Core.Services;
using MoneyManager.Models.Domain;
using System.Data;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
    }

    public class DapperUserRepository : DapperGenericRepository<User>, IUserRepository
    {
        public DapperUserRepository(IDbTransaction transaction, CurrentUserService currentUserService)
            : base(transaction, "User", currentUserService)
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await Connection.QuerySingleOrDefaultAsync<User>(
                @$"SELECT * FROM ""{TableName}"" WHERE Email=@email",
                new { email },
                Transaction
            );
        }
    }
}
