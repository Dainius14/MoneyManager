using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Core.Repositories
{
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetAsync(int id);

        public Task<int> SaveRangeAsync(IEnumerable<T> list);

        public Task UpdateAsync(T t);
        
        public Task<int> InsertAsync(T t);

        public Task DeleteAsync(int id);
    }
}
