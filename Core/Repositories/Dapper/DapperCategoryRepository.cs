using MoneyManager.Models.Domain;
using System.Data;

namespace MoneyManager.Core.Repositories.Dapper
{
    public class DapperCategoryRepository : DapperGenericRepository<Category>, ICategoryRepository
    {
        public DapperCategoryRepository(IDbTransaction transaction)
            : base(transaction, "category")
        {
        }
    }
}
