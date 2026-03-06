using Microsoft.EntityFrameworkCore;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Domain.Entities;
using Metrix.Domain.Enums;
using Metrix.Infrastructure.Persistence;

namespace Metrix.Infrastructure.Repositories;

public class AdminDashboardRepository : IAdminDashboardRepository
{
    private readonly AppDbContext _context;

    public AdminDashboardRepository(AppDbContext context) => _context = context;

    // ── Security users ────────────────────────────────────────────────────────

    public async Task<List<SecurityUser>> GetAllSecurityUsersAsync()
        => await _context.SecurityUsers
                         .AsNoTracking()
                         .OrderByDescending(u => u.CreatedAt)
                         .ToListAsync();

    public async Task<bool> SecurityEmailExistsAsync(string email)
        => await _context.SecurityUsers
                         .AnyAsync(u => u.Email == email.ToLower());

    public async Task<SecurityUser> CreateSecurityUserAsync(SecurityUser user)
    {
        _context.SecurityUsers.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeactivateSecurityUserAsync(int id)
    {
        var user = await _context.SecurityUsers.FindAsync(id);
        if (user is null) return;

        user.IsActive = false;
        await _context.SaveChangesAsync();
    }

    // ── Visitors ──────────────────────────────────────────────────────────────

    public async Task<List<Visitor>> GetAllVisitorsAsync()
        => await _context.Visitors
                         .Include(v => v.Invitation)
                         .ThenInclude(i => i!.CreatedByHR)
                         .Include(v => v.ApprovedByHR)
                         .AsNoTracking()
                         .OrderByDescending(v => v.SubmittedAt)
                         .ToListAsync();

    // ── Stats ─────────────────────────────────────────────────────────────────

    public async Task<int> CountSecurityUsersAsync()
        => await _context.SecurityUsers.CountAsync();

    public async Task<int> CountActiveSecurityUsersAsync()
        => await _context.SecurityUsers.CountAsync(u => u.IsActive);

    public async Task<int> CountTotalVisitorsAsync()
        => await _context.Visitors.CountAsync();

    public async Task<int> CountCheckedInTodayAsync()
    {
        var today = DateTime.UtcNow.Date;

        return await _context.VisitLogs.CountAsync(v =>
            v.EntryTime.Date == today &&
            v.ExitTime == null
        );
    }
    public async Task<int> CountCheckedOutTodayAsync()
    {
        var today = DateTime.UtcNow.Date;

        return await _context.VisitLogs.CountAsync(v =>
            v.ExitTime != null &&
            v.ExitTime.Value.Date == today
        );
    }
}