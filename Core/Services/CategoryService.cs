using MoneyManager.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;
using MoneyManager.Core.Services.Exceptions;

namespace MoneyManager.Core.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork _uow;
        private readonly int _currentUserId;

        public CategoryService(IUnitOfWork unitOfWork, CurrentUserService currentUser)
        {
            _uow = unitOfWork;
            _currentUserId = currentUser.Id;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return await _uow.CategoryRepo.GetAllAsync();
        }

        public async Task<Category> CreateAsync(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            category.UserId = _currentUserId;
            try
            {
                int categoryId = await _uow.CategoryRepo.InsertAsync(category);
                category.Id = categoryId;
                return category;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<Category> UpdateAsync(int id, Category category)
        {
            var existingCategory = await _uow.CategoryRepo.GetAsync(id);
            if (existingCategory == null)
            {
                throw new NotFoundException("Category not found");
            }

            existingCategory.Name = category.Name;
            existingCategory.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _uow.CategoryRepo.UpdateAsync(existingCategory);
                _uow.Commit();
                return existingCategory;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when updating the category: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            var existingCategory = await _uow.CategoryRepo.GetAsync(id);
            if (existingCategory == null)
            {
                throw new NotFoundException("Category not found");
            }

            try
            {
                await _uow.CategoryRepo.DeleteAsync(id);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred when deleting the category: {ex.Message}");
            }
        }
    }
}
