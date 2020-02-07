﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Core.Services;
using MoneyManager.Models.DTO;
using MoneyManager.Models.Mappers;
using MoneyManager.Web.Extensions;

namespace MoneyManager.Web.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.ListAsync();

            var categoriesDto = categories.Select(d => d.ToGetCategoryDTO());
            return Ok(categoriesDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] EditCategoryDTO inputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var category = inputDto.ToDomainModel();
            var result = await _categoryService.UpdateAsync(id, category);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var outputDto = result.Item!.ToGetCategoryDTO();
            return Ok(outputDto);
        }

        [HttpPost]
        public async Task<IActionResult> PostCategory([FromBody] EditCategoryDTO inputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var category = inputDto.ToDomainModel();
            var result = await _categoryService.CreateAsync(category);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var outputDto = result.Item!.ToGetCategoryDTO();
            return Ok(outputDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Message);
            }
            return NoContent();
        }
    }
}
