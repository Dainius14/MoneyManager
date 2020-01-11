using MoneyManager.Client.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.AspNetCore.Components;
using MoneyManager.Models.Domain;
using System.Linq;
using MoneyManager.Models.Mappers;
using MoneyManager.Models.DTO;

namespace MoneyManager.Client.Services
{
    public class RESTTransactionService : ITransactionService
    {
        private HttpClient _httpClient;

        public RESTTransactionService()
        {
            _httpClient = new HttpClient();
            //_httpClient.BaseAddress = new System.Uri("https://localhost:44358");
            _httpClient.BaseAddress = new System.Uri("https://localhost:5501");
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            var dtos = await _httpClient.GetJsonAsync<List<GetTransactionDTO>>("/api/transactions");
            var items = dtos.Select(item => item.ToDomainModel()).ToList();
            return items;
        }
        public async Task<Transaction> GetTransactionAsync(int transactionId)
        {
            var dto = await _httpClient.GetJsonAsync<GetTransactionDTO>("/api/transactions/" + transactionId);
            var item = dto.ToDomainModel();
            return item;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction transaction)
        {
            var dto = await _httpClient.PostJsonAsync<GetTransactionDTO>("/api/transactions", transaction);
            var item = dto.ToDomainModel();
            return item;
        }
        public async Task<Transaction> EditTransactionAsync(Transaction transaction)
        {
            var dto = await _httpClient.PutJsonAsync<GetTransactionDTO>("/api/transactions/" + transaction.ID, transaction);
            var item = dto.ToDomainModel();
            return item;
        }

        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            return (await _httpClient.DeleteAsync("/api/transactions/" + transactionId)).IsSuccessStatusCode;
        }

    }
}
