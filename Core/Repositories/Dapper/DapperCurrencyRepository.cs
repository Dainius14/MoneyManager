using MoneyManager.Models.Domain;
using System.Data;

namespace MoneyManager.Core.Repositories.Dapper
{
    public class DapperCurrencyRepository : DapperGenericRepository<Currency>, ICurrencyRepository
    {
        public DapperCurrencyRepository(IDbTransaction transaction)
            : base(transaction, "currency")
        {
        }
    }
}
