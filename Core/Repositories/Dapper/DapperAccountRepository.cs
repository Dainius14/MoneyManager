using Dapper;
using MoneyManager.Models.Domain;
using System.Data;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories.Dapper
{
    public class DapperAccountRepository : DapperGenericRepository<Account>, IAccountRepository
    {
        public DapperAccountRepository(IDbTransaction transaction)
            : base(transaction, "account")
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
