using MoneyManager.Models.Domain;
using MoneyManager.Models.DTO;
using MoneyManager.Models.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace MoneyManager.Client.Services
{
    public class TransactionService
    {
        private readonly HttpClient _httpClient;

        public TransactionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Transaction>?> GetAllTransactionsAsync()
        {
            List<GetTransactionDTO> response;
            try
            {
                response = await _httpClient.GetAsync<List<GetTransactionDTO>>("/transactions");
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            var transactions = response.Select(t => t.ToDomainModel()).ToList();
            return transactions;
        }

        public async Task<Transaction?> CreateTransactionAsync(Transaction givenItem)
        {
            var sendDto = givenItem.ToEditTransactionDTO();
            GetTransactionDTO response;
            try
            {
                response = await _httpClient.PostAsync<GetTransactionDTO>("/transactions", sendDto);
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            var transaction = response.ToDomainModel();
            return transaction;
        }

        public async Task<bool> DeleteTransactionAsync(int id)
        {
            try
            {
                return await _httpClient.DeleteAsync<bool>("/transactions/" + id);
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return false;
            }
        }
    }
}
