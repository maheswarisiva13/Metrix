using Metrix.Application.Interfaces.Repositories;
using Metrix.Domain.Entities;
using Metrix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

public class HRRepository : IHRRepository
{
    private readonly AppDbContext _context;

    public HRRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<HRUser?> GetByEmailAsync(string email)
    {
        return await _context.HRUsers.FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task AddAsync(HRUser user)
    {
        await _context.HRUsers.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
