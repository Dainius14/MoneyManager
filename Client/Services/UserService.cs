using System.Threading.Tasks;
using MoneyManager.Models.Mappers;
using System;
using MoneyManager.Models.DTO;
using MoneyManager.Models.ViewModels;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Actions;

namespace MoneyManager.Client.Services
{
    public class UserService
    {
        private static HttpClient _httpService = new HttpClient();
        private readonly Store<AppState> _store;
        private readonly JwtAuthStateProvider _authProvider;

        public UserService(Store<AppState> store, JwtAuthStateProvider authProvider)
        {
            _store = store;
            _authProvider = authProvider;
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
