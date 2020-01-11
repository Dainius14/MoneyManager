using MoneyManager.Client.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using MoneyManager.Models.Domain;
using System.Linq;
using MoneyManager.Models.Mappers;
using System;
using MoneyManager.Models.DTO;

namespace MoneyManager.Client.Services
{
    public static class TransactionService
    {
        private static HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:5501")
        };

        public static async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            var dtos = await _httpClient.GetJsonAsync<List<GetTransactionDTO>>("api/transactions");
            var items = dtos.Select(item => item.ToDomainModel()).ToList();
            return items;
        }

        public static async Task<Transaction> GetTransactionAsync(int id)
        {
            var dto = await _httpClient.GetJsonAsync<GetTransactionDTO>("api/transactions/" + id);
            var item = dto.ToDomainModel();
            return item;
        }

        public static async Task<Transaction> CreateTransactionAsync(Transaction givenItem)
        {
            var sendDto = givenItem.ToEditTransactionDTO();
            var dto = await _httpClient.PostJsonAsync<GetTransactionDTO>("api/transactions", sendDto);
            return dto.ToDomainModel();
        }

        public static async Task<bool> DeleteTransactionAsync(int id)
        {
            var request = await _httpClient.DeleteAsync("api/transactions/" + id);
            return request.IsSuccessStatusCode;
        }
    }
}
