
using ECommerce.API.DTOs.Product;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] int? categoryId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 || pageSize < 1 || pageSize > 50)
            {
                return BadRequest(
                    "Page number must be positive and page size must be between 1 and 50.");
            }

            if (categoryId.HasValue && categoryId.Value < 1)
            {
                return BadRequest("Invalid category ID.");
            }

            var products = await _service.GetAllAsync(
                search, categoryId, pageNumber, pageSize);

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
            {
                return BadRequest("Invalid product ID.");
            }

            var product = await _service.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] ProductCreateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                return BadRequest("Product name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return BadRequest("Product description is required.");
            }

            var product = await _service.CreateAsync(request);

            if (product == null)
            {
                return BadRequest("The selected category does not exist.");
            }

            return Ok(product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] ProductUpdateRequest request)
        {
            if (id < 1)
            {
                return BadRequest("Invalid product ID.");
            }

            if (string.IsNullOrWhiteSpace(request.ProductName))
            {
                return BadRequest("Product name is required.");
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                return BadRequest("Product description is required.");
            }

            var product = await _service.UpdateAsync(id, request);

            if (product == null)
            {
                return NotFound(
                    "Product or selected category was not found.");
            }

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest("Invalid product ID.");
            }

            var result = await _service.DeleteAsync(id);

            if (result == null)
            {
                return NotFound("Product not found.");
            }

            if (result.Value)
            {
                return Ok(new
                {
                    message = "Product has been deactivated because it is used in cart or order records."
                });
            }

            return NoContent();
        }
    }
}
