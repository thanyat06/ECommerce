namespace ECommerce.API.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        public int CategoryId { get; set; }

        public bool IsActive { get; set; } = true;

        public Category? Category { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}