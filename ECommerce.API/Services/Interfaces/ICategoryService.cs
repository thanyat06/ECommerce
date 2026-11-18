using ECommerce.API.DTOs.Category;

namespace ECommerce.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetAllAsync();

        Task<CategoryResponse?> GetByIdAsync(int id);

        Task<bool> CategoryNameExistsAsync(string name, int? excludeId = null);

        Task<bool> HasProductsAsync(int categoryId);

        Task<CategoryResponse> AddAsync(CategoryCreateRequest request);

        Task<CategoryResponse?> UpdateAsync(
            int id, CategoryUpdateRequest request);

        Task DeleteAsync(int id);
    }
}