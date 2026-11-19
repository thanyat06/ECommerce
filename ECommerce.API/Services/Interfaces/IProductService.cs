
using ECommerce.API.DTOs.Product;

namespace ECommerce.API.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductListResponse> GetAllAsync(
            string? search,
            int? categoryId,
            int pageNumber,
            int pageSize);

        Task<ProductResponse?> GetByIdAsync(int id);

        Task<ProductResponse?> CreateAsync(ProductCreateRequest request);

        Task<ProductResponse?> UpdateAsync(
            int id,
            ProductUpdateRequest request);

        Task<bool?> DeleteAsync(int id);
    }
}
