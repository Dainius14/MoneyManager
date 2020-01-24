using System.Threading.Tasks;
using MoneyManager.Models.Mappers;
using System;
using MoneyManager.Models.DTO;
using MoneyManager.Models.ViewModels;
using MoneyManager.Client.State;
using MoneyManager.Models.Domain;

namespace MoneyManager.Client.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly Store<AppState> _store;
        private readonly SessionStorage _sessionStorage;
        private readonly LocalStorage _localStorage;

        public UserService(Store<AppState> store, LocalStorage localStorage, SessionStorage sessionStorage,
            HttpClient httpClient)
        {
            _store = store;
            _localStorage = localStorage;
            _sessionStorage = sessionStorage;
            _httpClient = httpClient;

            Console.WriteLine("UserService. Constructing");
        }
    }
}
