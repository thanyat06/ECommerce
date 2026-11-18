using System.ComponentModel.DataAnnotations;

namespace ECommerce.API.DTOs.Category
{
    public class CategoryUpdateRequest
    {
        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}