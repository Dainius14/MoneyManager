using MoneyManager.Core.Services.Communication;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;

namespace MoneyManager.Core.Services
{
    public interface ICurrencyService
    {
        Task<IEnumerable<Currency>> ListAsync();
    }
}
