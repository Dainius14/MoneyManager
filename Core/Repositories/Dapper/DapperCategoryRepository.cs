using MoneyManager.Core.Services;
using MoneyManager.Models.Domain;
using System.Data;

namespace MoneyManager.Core.Repositories.Dapper
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
    }

    public class DapperCategoryRepository : DapperGenericRepository<Category>, ICategoryRepository
    {
        public DapperCategoryRepository(IDbTransaction transaction, CurrentUserService currentUserService)
            : base(transaction, "category", currentUserService)
        {
        }
    }
}
