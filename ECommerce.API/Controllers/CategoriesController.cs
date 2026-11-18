using ECommerce.API.DTOs.Category;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();

            return Ok(categories);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> Create(
            CategoryCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest("Category name is required.");

            if (await _categoryService.CategoryNameExistsAsync(
                request.CategoryName))
            {
                return Conflict("Category already exists.");
            }

            var category = await _categoryService.AddAsync(request);

            return Ok(category);
        }

        // PUT: api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id, CategoryUpdateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.CategoryName))
                return BadRequest("Category name is required.");

            var existing = await _categoryService.GetByIdAsync(id);

            if (existing == null)
                return NotFound("Category not found.");

            if (await _categoryService.CategoryNameExistsAsync(
                request.CategoryName, id))
            {
                return Conflict("Category name already exists.");
            }

            var category = await _categoryService.UpdateAsync(id, request);

            return Ok(category);
        }

        // DELETE: api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
                return NotFound("Category not found.");

            if (await _categoryService.HasProductsAsync(id))
            {
                return BadRequest(
                    "Cannot delete category because products are associated with it.");
            }

            await _categoryService.DeleteAsync(id);

            return NoContent();
        }
    }
}