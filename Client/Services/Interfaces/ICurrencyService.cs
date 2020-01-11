using MoneyManager.Models.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<Currency>> GetAllCurrenciesAsync();
    }
}
