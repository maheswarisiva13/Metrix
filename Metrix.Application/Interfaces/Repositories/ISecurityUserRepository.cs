using Metrix.Domain.Entities;

public interface ISecurityUserRepository
{
    Task<SecurityUser?> GetByEmailAsync(string email);
    Task AddAsync(SecurityUser user);
}
