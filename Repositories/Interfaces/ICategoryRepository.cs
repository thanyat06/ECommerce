using ECommerce.API.Models;

namespace ECommerce.API.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<bool> CategoryNameExistsAsync(string name, int? excludeId = null);

        Task<bool> HasProductsAsync(int categoryId);

        Task AddAsync(Category category);

        Task UpdateAsync(Category category);

        Task DeleteAsync(Category category);
    }
}