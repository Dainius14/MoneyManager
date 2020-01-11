using MoneyManager.Core.Services.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;

namespace MoneyManager.Core.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> ListAsync();
        Task<Response<Category>> CreateAsync(Category category);
        Task<Response<Category>> UpdateAsync(int id, Category category);
        Task<Response<Category>> DeleteAsync(int id);
    }
}
