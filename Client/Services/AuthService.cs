using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace MoneyManager.Client.Services
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly SessionStorage _sessionStorage;
        private readonly LocalStorage _localStorage;

        private Task<bool>? _refreshTokenTask;

        public delegate void AuthenticatedHandler(Dictionary<string, object> accessTokenPayload);
        public event AuthenticatedHandler? OnAuthenticated;

        public AuthService(
            HttpClient httpClient,
            LocalStorage localStorage,
            SessionStorage sessionStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _sessionStorage = sessionStorage;

            _httpClient.AccessTokenExpired += TryAuthenticateFromRefreshToken;
        }

        public async Task<bool> TryAuthenticate()
        {
            return TryAuthenticateFromAccessToken() || await TryAuthenticateFromRefreshToken();
        }

        public bool TryAuthenticateFromAccessToken()
        {
            var accessToken = GetAccessTokenFromSessionStorage();
            if (!IsAccessTokenValid(accessToken, out Dictionary<string, object>? payload))
            {
                return false;
            }

            _httpClient.SetAuthHeader(accessToken!);
            FireOnAuthenticatedEvent(accessToken!);
            return true;
        }


        public Task<bool> TryAuthenticateFromRefreshToken()
        {
            var accessToken = GetAccessTokenFromSessionStorage();
            var refreshToken = GetRefreshTokenFromLocalStorage();
            if (string.IsNullOrEmpty(accessToken) || string.IsNullOrEmpty(refreshToken))
            {
                return Task.FromResult(false);
            }

            if (_refreshTokenTask == null || _refreshTokenTask.IsCompleted)
            {                
                _refreshTokenTask = AuthenticateFromRefreshToken(accessToken, refreshToken);
            }
            return _refreshTokenTask;
        }

        private async Task<bool> AuthenticateFromRefreshToken(string accessToken, string refreshToken)
        {
            var response = await RefreshAccessTokenAsync(accessToken, refreshToken);
            if (response == null)
            {
                return false;
            }

            _httpClient.SetAuthHeader(response.AccessToken);
            SaveAccessTokenToSessionStorage(response.AccessToken);
            SaveRefreshTokenToLocalStorage(response.RefreshToken);
            FireOnAuthenticatedEvent(response.AccessToken);
            return true;
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
                Console.WriteLine("AuthService AuthenticateAsync()  HttpException: " + ex.Message);
                return false;
            }

            var vm = responseDto.ToViewModel();

            SaveAccessTokenToSessionStorage(vm.AccessToken);
            SaveRefreshTokenToLocalStorage(vm.RefreshToken);
            _httpClient.SetAuthHeader(vm.AccessToken);
            FireOnAuthenticatedEvent(vm.AccessToken);


            return true;
        }

        public async Task<RefreshTokenVm?> RefreshAccessTokenAsync(string accessToken, string refreshToken)
        {
            var requestDto = new RefreshTokenVm(accessToken, refreshToken).ToDto();

            RefreshTokenVmDto responseDto;
            try
            {
                responseDto = await _httpClient.PostAsync<RefreshTokenVmDto>("/users/refreshToken", requestDto);
            }
            catch (HttpAuthException ex)
            {
                Console.WriteLine("AuthService RefreshAccessTokenAsync()  HttpAuthException: " + ex.Message);
                return null;
            }
            catch (HttpException ex)
            {
                Console.WriteLine("AuthService RefreshAccessTokenAsync()  HttpException: " + ex.Message);
                return null;
            }

            Console.WriteLine("AuthService RefreshAccessTokenAsync()  Ok");

            return responseDto.ToViewModel();
        }

        private void FireOnAuthenticatedEvent(string accessToken)
        {
            var payload = GetJwtPayload(accessToken);
            OnAuthenticated?.Invoke(payload);
        }


        public string? GetAccessTokenFromSessionStorage() =>
            _sessionStorage.GetItem<string?>("accessToken");

        private string? GetRefreshTokenFromLocalStorage() =>
            _localStorage.GetItem<string?>("refreshToken");


        private void SaveAccessTokenToSessionStorage(string token) =>
            _sessionStorage.SetItem("accessToken", token);

        private void SaveRefreshTokenToLocalStorage(string token) =>
            _localStorage.SetItem("refreshToken", token);


        private bool IsAccessTokenValid(string? token, out Dictionary<string, object>? payload)
        {
            if (string.IsNullOrEmpty(token))
            {
                payload = null;
                return false;
            }

            payload = GetJwtPayload(token);
            payload.TryGetValue("exp", out object expiryObject);
            var expiry = DateTimeOffset.FromUnixTimeSeconds(int.Parse(expiryObject.ToString())).UtcDateTime;
            return expiry > DateTime.UtcNow;
        }

        public Dictionary<string, object> GetJwtPayload(string jwt)
        {
            string payload = jwt.Split('.')[1];
            byte[] jsonBytes = ParseBase64WithoutPadding(payload);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

    }
}
