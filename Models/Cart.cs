namespace ECommerce.API.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public int UserId { get; set; }

        public ApplicationUser? User { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}