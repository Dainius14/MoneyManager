using System.Threading.Tasks;
using MoneyManager.Models.Mappers;
using System;
using MoneyManager.Models.DTO;
using MoneyManager.Models.ViewModels;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Actions;
using System.Dynamic;

namespace MoneyManager.Client.Services
{
    public class UserService
    {
        private static HttpClient _httpService = new HttpClient();
        private readonly Store<AppState> _store;
        private readonly JwtAuthStateProvider _authProvider;
        private readonly LocalStorage _localStorage;

        public UserService(Store<AppState> store, JwtAuthStateProvider authProvider, LocalStorage localStorage)
        {
            _store = store;
            _authProvider = authProvider;
            _localStorage = localStorage;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            var data = new AuthenticateDto(email, password);

            try
            {
                var response = await _httpService.Post<AuthenticatedUserVmDto>("/users/authenticate", data);
                var vm = response.ToViewModel();
                
                //_store.Dispath(new UserActions.Set(vm.User));
                _httpService.SetAuthHeader(vm.Token);
                _authProvider.User = vm.User;
                await _localStorage.SetItemAsync("authToken", vm.Token);
                Console.WriteLine("Success: " + response.Token);
                return true;
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }

    }
}
