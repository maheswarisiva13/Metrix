using Metrix.Application.Interfaces.Repositories;
using Metrix.Domain.Entities;
using Metrix.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Metrix.Infrastructure.Repositories;

public class AdminRepository : IAdminRepository
{
    private readonly AppDbContext _context;

    public AdminRepository(AppDbContext context)
    {
        _context = context;
    }

    // ================= CHECK ADMIN EXISTS =================
    public async Task<bool> AdminExistsAsync()
    {
        return await _context.AdminUsers.AnyAsync();
    }

    // ================= ADD ADMIN =================
    public async Task AddAdminAsync(User admin)
    {
        await _context.AdminUsers.AddAsync(admin);
    }

    // ================= GET ADMIN BY EMAIL =================
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.AdminUsers
            .FirstOrDefaultAsync(a => a.Email == email);
    }

    // ================= CHECK SECURITY EMAIL =================
    public async Task<bool> SecurityEmailExistsAsync(string email)
    {
        return await _context.SecurityUsers
            .AnyAsync(s => s.Email == email);
    }

    // ================= ADD SECURITY =================
    public async Task AddSecurityAsync(SecurityUser security)
    {
        await _context.SecurityUsers.AddAsync(security);
    }

    // ================= SAVE CHANGES =================
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}