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
    public static class CategoryService
    {
        private static HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:5501")
        };
        


        public static async Task<List<Category>> GetAllCategoriesAsync()
        {
            var dtos = await _httpClient.GetJsonAsync<List<GetCategoryDTO>>("api/categories");
            var items = dtos.Select(item => item.ToDomainModel()).ToList();
            return items;
        }

        public static async Task<Category> GetCategoryAsync(int id)
        {
            var dto = await _httpClient.GetJsonAsync<GetCategoryDTO>("api/categories/" + id);
            var item = dto.ToDomainModel();
            return item;
        }

        public static async Task<Category> CreateCategoryAsync(Category givenItem)
        {
            var sendDto = givenItem.ToEditCategoryDTO();
            var dto = await _httpClient.PostJsonAsync<GetCategoryDTO>("api/categories", sendDto);
            return dto.ToDomainModel();
        }
    }
}
