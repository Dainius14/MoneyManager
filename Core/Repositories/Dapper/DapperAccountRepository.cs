using Dapper;
using MoneyManager.Core.Services;
using MoneyManager.Models.Domain;
using System.Data;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public interface IAccountRepository : IGenericRepository<Account>
    {
        public Task<Account> GetByNameAsync(int userId, string name, bool isPersonal);
    }

    public class DapperAccountRepository : DapperGenericRepository<Account>, IAccountRepository
    {
        public DapperAccountRepository(IDbTransaction transaction, CurrentUserService currentUserService)
            : base(transaction, "account", currentUserService)
        {

        }
        public async Task<Account> GetByNameAsync(int userId, string name, bool isPersonal)
        {
            return await Connection.QuerySingleOrDefaultAsync<Account>(
                @$"
                SELECT *
                FROM ""{TableName}""
                WHERE UserId=@userId AND Name=@name AND IsPersonal=@isPersonal
                ",
                new { userId, name, isPersonal },
                Transaction
            );
        }
    }
}
