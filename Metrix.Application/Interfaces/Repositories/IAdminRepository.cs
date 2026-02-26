using Metrix.Domain.Entities;

namespace Metrix.Application.Interfaces.Repositories;

public interface IAdminRepository
{
    Task<bool> AdminExistsAsync();
    Task AddAdminAsync(User admin);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> SecurityEmailExistsAsync(string email);
    Task AddSecurityAsync(SecurityUser security);
    Task SaveChangesAsync();
}