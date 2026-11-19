
namespace ECommerce.API.DTOs.Product
{
    public class ProductListResponse
    {
        public List<ProductResponse> Products { get; set; } = new();

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
    }
}
