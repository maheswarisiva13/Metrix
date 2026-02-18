using Metrix.Domain.Entities;

namespace Metrix.Application.Interfaces.Repositories;

public interface IHRRepository
{
    Task<HRUser?> GetByEmailAsync(string email);
    Task AddAsync(HRUser user);
}
