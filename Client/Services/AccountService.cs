using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MoneyManager.Models.Mappers;
using MoneyManager.Client.State;
using System;
using MoneyManager.Models.ViewModels;

namespace MoneyManager.Client.Services
{
    public class AccountService
    {

        private readonly HttpClient _httpClient;
        private readonly Store<AppState> _store;

        public AccountService(Store<AppState> store, HttpClient httpClient)
        {
            _store = store;
            _httpClient = httpClient;
        }

        public async Task<List<GetPersonalAccountVm>?> GetAllPersonalAccountsAsync()
        {
            List<GetPersonalAccountVmDto> dtos;
            try
            {
                dtos = await _httpClient.GetAsync<List<GetPersonalAccountVmDto>>("/accounts/personal");
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            var items = dtos.Select(item => item.ToViewModel()).ToList();
            return items;
        }

        public async Task<List<GetNonPersonalAccountVm>?> GetAllNonPersonalAccountsAsync()
        {
            List<GetNonPersonalAccountVmDto> dtos;
            try
            {
                dtos = await _httpClient.GetAsync<List<GetNonPersonalAccountVmDto>>("/accounts/nonpersonal");
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            var items = dtos.Select(item => item.ToViewModel()).ToList();
            return items;
        }

        public async Task<GetPersonalAccountVm?> CreatePersonalAccountAsync(EditPersonalAccountVm vm)
        {
            GetPersonalAccountVmDto dto;
            try
            {
                dto = await _httpClient.PostAsync<GetPersonalAccountVmDto>("/accounts/personal", vm.ToDto());
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            return dto.ToViewModel();
        }

        public async Task<GetNonPersonalAccountVm?> CreateNonPersonalAccountAsync(EditNonPersonalAccountVm vm)
        {
            GetNonPersonalAccountVmDto dto;
            try
            {
                dto = await _httpClient.PostAsync<GetNonPersonalAccountVmDto>("/accounts/nonpersonal", vm.ToDto());
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            return dto.ToViewModel();
        }
    }
}
