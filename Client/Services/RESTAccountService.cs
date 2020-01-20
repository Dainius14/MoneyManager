using MoneyManager.Client.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using MoneyManager.Models.Domain;
using System.Linq;
using MoneyManager.Models.Mappers;
using MoneyManager.Client.State;
using MoneyManager.Client.State.Actions;
using System;
using MoneyManager.Models.ViewModels;
using MoneyManager.Models.DTO;

namespace MoneyManager.Client.Services
{
    public class RESTAccountService : IAccountService
    {
        private static System.Net.Http.HttpClient _httpClient = new System.Net.Http.HttpClient()
        {
            BaseAddress = new Uri("https://localhost:5501")
        };

        private Store<AppState> _store;

        public RESTAccountService(Store<AppState> store)
        {
            _store = store;
        }

        async Task<List<Account>> IAccountService.GetAllAccountsAsync() =>
            await GetAllAccountsAsync();


        public static async Task<List<GetPersonalAccountVm>> GetAllPersonalAccountsAsync()
        {
            var dtos = await _httpClient.GetJsonAsync<List<GetPersonalAccountVmDto>>("api/accounts/personal");
            var items = dtos.Select(item => item.ToViewModel()).ToList();
            return items;
        }

        public static async Task<List<GetNonPersonalAccountVm>> GetAllNonPersonalAccountsAsync()
        {
            var dtos = await _httpClient.GetJsonAsync<List<GetNonPersonalAccountVmDto>>("api/accounts/nonpersonal");
            var items = dtos.Select(item => item.ToViewModel()).ToList();
            return items;
        }

        public static async Task<GetPersonalAccountVm> CreatePersonalAccountAsync(EditPersonalAccountVm vm)
        {
            var dto = await _httpClient.PostJsonAsync<GetPersonalAccountVmDto>("api/accounts/personal", vm.ToDto());
            var item = dto.ToViewModel();
            return item;
        }
        public static async Task<GetNonPersonalAccountVm> CreateNonPersonalAccountAsync(EditNonPersonalAccountVm vm)
        {
            var dto = await _httpClient.PostJsonAsync<GetNonPersonalAccountVmDto>("api/accounts/nonpersonal", vm.ToDto());
            var item = dto.ToViewModel();
            return item;
        }






        public static async Task<List<Account>> GetAllAccountsAsync()
        {
            var dtos = await _httpClient.GetJsonAsync<List<GetAccountDTO>>("api/accounts");
            var items = dtos.Select(item => item.ToDomainModel()).ToList();
            return items;
        }


        public async Task<Account> GetAccountAsync(int accountId)
        {
            var dto = await _httpClient.GetJsonAsync<GetAccountDTO>("api/accounts/" + accountId);
            var item = dto.ToDomainModel();
            return item;
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            var dto = await _httpClient.PostJsonAsync<GetAccountDTO>("api/accounts", account);
            var item = dto.ToDomainModel();
            return item;
        }

        public async Task<bool> DeleteAccountAsync(int accountId)
        {
            return (await _httpClient.DeleteAsync("api/accounts/" + accountId)).IsSuccessStatusCode;
        }

        async Task<Account> IAccountService.EditAccountAsync(Account account) =>
            await EditAccountAsync(account);

        public static async Task<Account> EditAccountAsync(Account account)
        {
            throw new NotImplementedException();
            //var editDto = account.ToEditAccountDTO();
            //var dto = await _httpClient.PutJsonAsync<GetAccountDTO>("api/accounts/" + account.Id, editDto);
            //var item = dto.ToDomainModel();

            //return item;
        }

        public static async Task<EditPersonalAccountVm> CreatePersonalAccount(EditPersonalAccountVm createVm)
        {
            return await _httpClient.PostJsonAsync<EditPersonalAccountVm>("api/accounts/personal", createVm);
        }
    }
}
