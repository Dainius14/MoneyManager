using MoneyManager.Client.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using MoneyManager.Models.Domain;
using System.Linq;
using MoneyManager.Models.Mappers;
using MoneyManager.Client.State;
using System;
using MoneyManager.Models.DTO;

namespace MoneyManager.Client.Services
{
    public class RESTCurrencyService : ICurrencyService
    {
        private static HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:5501")
        };

        private IStore<AppState> _store;

        public RESTCurrencyService(Store<AppState> store)
        {
            _store = store;
        }

        async Task<List<Currency>> ICurrencyService.GetAllCurrenciesAsync() =>
            await GetAllCurrenciesAsync();
        public static async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            var dtos = await _httpClient.GetJsonAsync<List<GetCurrencyDTO>>("/api/currencies");
            var items = dtos.Select(item => item.ToDomainModel()).ToList();
            return items;
        }
    }
}
