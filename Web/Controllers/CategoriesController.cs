using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Core.Services;
using MoneyManager.Core.Services.Exceptions;
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
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
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

            try
            {
                var result = await _categoryService.UpdateAsync(id, category);
                var outputDto = result.ToGetCategoryDTO();
                return Ok(outputDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("{id}")]
        public async Task<IActionResult> PostCategory(int id, [FromBody] EditCategoryDTO inputDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.GetErrorMessages());
            }

            var category = inputDto.ToDomainModel();
            try
            {
                var result = await _categoryService.UpdateAsync(id, category);
                var outputDto = result.ToGetCategoryDTO();
                return Ok(outputDto);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
