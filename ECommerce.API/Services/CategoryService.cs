using ECommerce.API.DTOs.Category;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using ECommerce.API.Services.Interfaces;

namespace ECommerce.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<CategoryResponse>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Select(c => new CategoryResponse
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,
                Description = c.Description
            }).ToList();
        }

        public async Task<CategoryResponse?> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return null;

            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

        public async Task<bool> CategoryNameExistsAsync(
            string name, int? excludeId = null)
        {
            return await _categoryRepository.CategoryNameExistsAsync(
                name.Trim(), excludeId);
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await _categoryRepository.HasProductsAsync(categoryId);
        }

        public async Task<CategoryResponse> AddAsync(
            CategoryCreateRequest request)
        {
            var category = new Category
            {
                CategoryName = request.CategoryName.Trim(),
                Description = request.Description?.Trim()
            };

            await _categoryRepository.AddAsync(category);

            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

        public async Task<CategoryResponse?> UpdateAsync(
            int id, CategoryUpdateRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category == null)
                return null;

            category.CategoryName = request.CategoryName.Trim();
            category.Description = request.Description?.Trim();

            await _categoryRepository.UpdateAsync(category);

            return new CategoryResponse
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.Description
            };
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            if (category != null)
            {
                await _categoryRepository.DeleteAsync(category);
            }
        }
    }
}