using ECommerce.API.Models;

namespace ECommerce.API.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<ApplicationUser?> GetByEmailAsync(string email);
    Task AddAsync(ApplicationUser user);
}