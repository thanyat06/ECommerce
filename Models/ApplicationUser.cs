namespace ECommerce.API.Models
{
    public class ApplicationUser
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Customer";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Cart? Cart { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}