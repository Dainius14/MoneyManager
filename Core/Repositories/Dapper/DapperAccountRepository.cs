using MoneyManager.Models.Domain;
using System.Data;

namespace MoneyManager.Core.Repositories.Dapper
{
    public class DapperAccountRepository : DapperGenericRepository<Account>, IAccountRepository
    {
        public DapperAccountRepository(IDbTransaction transaction)
            : base(transaction, "account")
        {
        }
    }
}
