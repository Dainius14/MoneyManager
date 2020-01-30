using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;
using System.Linq;
using MoneyManager.Models.Mappers;
using System;
using MoneyManager.Models.DTO;

namespace MoneyManager.Client.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Category>?> GetAllCategoriesAsync()
        {
            List<GetCategoryDTO> response;
            try
            {
                response = await _httpClient.GetAsync<List<GetCategoryDTO>>("/categories");
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            var items = response.Select(t => t.ToDomainModel()).ToList();
            return items;
        }

        //public async Task<Category> GetCategoryAsync(int id)
        //{
        //    var dto = await _httpClient.GetJsonAsync<GetCategoryDTO>("api/categories/" + id);
        //    var item = dto.ToDomainModel();
        //    return item;
        //}

        public async Task<Category?> CreateCategoryAsync(Category givenItem)
        {
            GetCategoryDTO response;
            try
            {
                response = await _httpClient.PostAsync<GetCategoryDTO>("/categories", givenItem.ToEditCategoryDTO());
            }
            catch (HttpException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
                return null;
            }
            var item = response.ToDomainModel();
            return item;
        }
    }
}
