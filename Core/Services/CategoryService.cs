﻿using MoneyManager.Core.Repositories;
using MoneyManager.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyManager.Models.Domain;

namespace MoneyManager.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _uow;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }

        public async Task<IEnumerable<Category>> ListAsync()
        {
            return (await _uow.CategoryRepo.GetAllAsync());
        }

        public async Task<Response<Category>> CreateAsync(Category category)
        {
            category.CreatedAt = DateTime.UtcNow;
            try
            {
                int categoryId = await _uow.CategoryRepo.InsertAsync(category);
                _uow.Commit();

                Category? parent = null;
                if (category.ParentID != null)
                {
                    parent = await _uow.CategoryRepo.GetAsync((int)category.ParentID);
                }

                var createdCategory = new Category
                {
                    ID = categoryId,
                    Name = category.Name,
                    Parent = parent,
                    ParentID = parent?.ID,
                    CreatedAt = category.CreatedAt
                };

                return new Response<Category>(createdCategory);
            }
            catch (Exception ex)
            {
                return new Response<Category>($"An error occurred when saving the category: {ex.Message}");
            }
        }

        public async Task<Response<Category>> UpdateAsync(int id, Category category)
        {
            var existingCategory = await _uow.CategoryRepo.GetAsync(id);
            if (existingCategory == null)
            {
                return new Response<Category>("Category not found");
            }

            existingCategory.Name = category.Name;
            existingCategory.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _uow.CategoryRepo.UpdateAsync(existingCategory);
                _uow.Commit();

                return new Response<Category>(existingCategory);
            }
            catch (Exception ex)
            {
                return new Response<Category>($"An error occurred when updating the category: {ex.Message}");
            }
        }

        public async Task<Response<Category>> DeleteAsync(int id)
        {
            var existingCategory = await _uow.CategoryRepo.GetAsync(id);
            if (existingCategory == null)
            {
                return new Response<Category>("Category not found");
            }

            try
            {
                await _uow.CategoryRepo.DeleteAsync(id);
                _uow.Commit();

                return new Response<Category>(existingCategory);
            }
            catch (Exception ex)
            {
                return new Response<Category>($"An error occurred when deleting the category: {ex.Message}");
            }
        }
    }
}
