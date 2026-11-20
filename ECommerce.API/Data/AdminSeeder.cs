using ECommerce.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Data;

public static class AdminSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services,
        IConfiguration configuration)
    {
        string? fullName = configuration["AdminSeed:FullName"];
        string? email = configuration["AdminSeed:Email"];
        string? password = configuration["AdminSeed:Password"];

        if (string.IsNullOrWhiteSpace(fullName) &&
            string.IsNullOrWhiteSpace(email) &&
            string.IsNullOrWhiteSpace(password))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(fullName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidOperationException(
                "Admin seed configuration is incomplete.");
        }

        var context = services.GetRequiredService<ApplicationDbContext>();

        email = email.Trim().ToLower();

        var existingUser = await context.ApplicationUsers
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email);

        if (existingUser != null)
        {
            if (existingUser.Role != "Admin")
            {
                throw new InvalidOperationException(
                    "The configured Admin email already belongs to a non-Admin user.");
            }

            return;
        }

        var admin = new ApplicationUser
        {
            FullName = fullName.Trim(),
            Email = email,
            Role = "Admin",
            CreatedAt = DateTime.UtcNow
        };

        var passwordHasher = new PasswordHasher<ApplicationUser>();

        admin.PasswordHash = passwordHasher.HashPassword(
            admin,
            password);

        context.ApplicationUsers.Add(admin);

        await context.SaveChangesAsync();
    }
}