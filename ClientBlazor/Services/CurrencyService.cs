using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;
using System.Linq;
using MoneyManager.Models.Mappers;
using MoneyManager.Client.State;
using System;
using MoneyManager.Models.DTO;

namespace MoneyManager.Client.Services
{
    public class CurrencyService
    {
        private readonly IStore<AppState> _store;
        private readonly HttpClient _httpClient;

        public CurrencyService(Store<AppState> store, HttpClient httpClient)
        {
            _store = store;
            _httpClient = httpClient;
        }

        public async Task<List<Currency>?> GetAllCurrenciesAsync()
        {
            List<GetCurrencyDTO> response;
            try
            {
                response = await _httpClient.GetAsync<List<GetCurrencyDTO>>("/currencies");
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            var items = response.Select(t => t.ToDomainModel()).ToList();
            return items;
        }
    }
}
