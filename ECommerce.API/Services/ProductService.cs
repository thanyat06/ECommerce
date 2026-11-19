
using ECommerce.API.DTOs.Product;
using ECommerce.API.Models;
using ECommerce.API.Repositories.Interfaces;
using ECommerce.API.Services.Interfaces;

namespace ECommerce.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProductListResponse> GetAllAsync(
            string? search,
            int? categoryId,
            int pageNumber,
            int pageSize)
        {
            var products = await _repository.GetAllAsync(
                search, categoryId, pageNumber, pageSize);

            int totalRecords = await _repository.GetTotalCountAsync(
                search, categoryId);

            return new ProductListResponse
            {
                Products = products.Select(MapToResponse).ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(
                    totalRecords / (double)pageSize)
            };
        }

        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            return product == null ? null : MapToResponse(product);
        }

        public async Task<ProductResponse?> CreateAsync(
            ProductCreateRequest request)
        {
            if (!await _repository.CategoryExistsAsync(request.CategoryId))
            {
                return null;
            }

            var product = new Product
            {
                ProductName = request.ProductName.Trim(),
                Description = request.Description.Trim(),
                Price = request.Price,
                Stock = request.Stock,
                ImageUrl = request.ImageUrl,
                CategoryId = request.CategoryId,
                IsActive = true
            };

            await _repository.AddAsync(product);

            var createdProduct = await _repository.GetByIdAsync(
                product.ProductId);

            return createdProduct == null
                ? null
                : MapToResponse(createdProduct);
        }

        public async Task<ProductResponse?> UpdateAsync(
            int id,
            ProductUpdateRequest request)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            if (!await _repository.CategoryExistsAsync(request.CategoryId))
            {
                return null;
            }

            product.ProductName = request.ProductName.Trim();
            product.Description = request.Description.Trim();
            product.Price = request.Price;
            product.Stock = request.Stock;
            product.ImageUrl = request.ImageUrl;
            product.CategoryId = request.CategoryId;
            

            await _repository.UpdateAsync(product);

            var updatedProduct = await _repository.GetByIdAsync(id);

            return updatedProduct == null
                ? null
                : MapToResponse(updatedProduct);
        }

        public async Task<bool?> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
            {
                return null;
            }

            if (await _repository.HasDependenciesAsync(id))
            {
                product.IsActive = false;
                await _repository.UpdateAsync(product);

                return true;
            }

            await _repository.DeleteAsync(product);

            return false;
        }

        private static ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.CategoryName ?? string.Empty,
                IsActive = product.IsActive
            };
        }
    }
}
