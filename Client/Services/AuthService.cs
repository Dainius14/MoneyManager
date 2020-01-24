using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly JwtAuthStateProvider _authProvider;
        private readonly SessionStorage _sessionStorage;
        private readonly LocalStorage _localStorage;

        public AuthService(JwtAuthStateProvider authProvider,
            LocalStorage localStorage, SessionStorage sessionStorage, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _authProvider = authProvider;
            _localStorage = localStorage;
            _sessionStorage = sessionStorage;

            Console.WriteLine("setting callback: " + _httpClient.GetHashCode());
            _httpClient.OnAuthTokenExpired = RefreshAuthToken;
        }

        public async Task<bool> AuthenticateAsync(string email, string password)
        {
            var data = new AuthenticateDto(email, password);

            AuthenticatedUserVmDto responseDto;
            try
            {
                responseDto = await _httpClient.PostAsync<AuthenticatedUserVmDto>("/users/authenticate", data);
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }

            var vm = responseDto.ToViewModel();

            _localStorage.SetItem("refreshToken", vm.RefreshToken);
            _sessionStorage.SetItem("authToken", vm.AuthToken);
            _httpClient.SetAuthHeader(vm.AuthToken);
            _authProvider.User = vm.User;
            return true;
        }

        public async Task<bool> RefreshAuthToken()
        {
            Console.WriteLine("RefreshAuthToken. Called");
            var requestDto = new RefreshTokenVm(
                _sessionStorage.GetItem<string>("authToken"),
                _localStorage.GetItem<string>("refreshToken")
            ).ToDto();

            RefreshTokenVmDto responseDto;
            try
            {
                responseDto = await _httpClient.PostAsync<RefreshTokenVmDto>("/users/refreshToken", requestDto);
            }
            catch (HttpAuthException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            Console.WriteLine("RefreshAuthToken. Setting new tokens");

            var vm = responseDto.ToViewModel();
            _localStorage.SetItem("refreshToken", vm.RefreshToken);
            _sessionStorage.SetItem("authToken", vm.AuthToken);
            _httpClient.SetAuthHeader(vm.AuthToken);

            if (!(await _authProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated)
            {
                var user = await GetCurrentUser();
                _authProvider.User = user;
            }

            return true;
        }

        public bool SetAuthTokenFromSession()
        {
            string? existingAuthToken = _sessionStorage.GetItem<string>("authToken");
            if (!string.IsNullOrEmpty(existingAuthToken))
            {
                _httpClient.SetAuthHeader(existingAuthToken);
                return true;
            }
            return false;
        }

        private async Task<User> GetCurrentUser()
        {
            var response = await _httpClient.GetAsync<GetUserDto>("/users/current");
            var user = response.ToDomainModel();
            return user;
        }
    }
}
