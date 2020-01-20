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
        private static HttpClient _httpClient2 = new HttpClient();

        public static async Task<List<Transaction>?> GetAllTransactionsAsync()
        {

            //var data = new AuthenticateDto(email, password);

            try
            {
                var response = await _httpClient2.Get<List<GetTransactionDTO>>("/transactions");
                var transactions = response.Select(t => t.ToDomainModel()).ToList();
                Console.WriteLine("transactions success");
                return transactions;
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            //var dtos = await _httpClient.GetJsonAsync<List<GetTransactionDTO>>("api/transactions");
            //var items = dtos.Select(item => item.ToDomainModel()).ToList();
            //return items;
        }

        public static async Task<Transaction> GetTransactionAsync(int id)
        {
            var _httpClient = new System.Net.Http.HttpClient();
            var dto = await _httpClient.GetJsonAsync<GetTransactionDTO>("api/transactions/" + id);
            var item = dto.ToDomainModel();
            return item;
        }

        public static async Task<Transaction> CreateTransactionAsync(Transaction givenItem)
        {
            var _httpClient = new System.Net.Http.HttpClient();
            var sendDto = givenItem.ToEditTransactionDTO();
            var dto = await _httpClient.PostJsonAsync<GetTransactionDTO>("api/transactions", sendDto);
            return dto.ToDomainModel();
        }

        public static async Task<bool> DeleteTransactionAsync(int id)
        {
            var _httpClient = new System.Net.Http.HttpClient();
            var request = await _httpClient.DeleteAsync("api/transactions/" + id);
            return request.IsSuccessStatusCode;
        }
    }
}
