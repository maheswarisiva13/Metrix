using Microsoft.EntityFrameworkCore;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Domain.Entities;
using Metrix.Infrastructure.Persistence;

public class SecurityUserRepository : ISecurityUserRepository
{
    private readonly AppDbContext _context;

    public SecurityUserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SecurityUser?> GetByEmailAsync(string email)
        => await _context.SecurityUsers
            .FirstOrDefaultAsync(x => x.Email == email);

    public async Task AddAsync(SecurityUser user)
    {
        await _context.SecurityUsers.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
