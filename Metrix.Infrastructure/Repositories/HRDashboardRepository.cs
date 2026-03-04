using Microsoft.EntityFrameworkCore;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Domain.Entities;
using Metrix.Domain.Enums;
using Metrix.Infrastructure.Persistence;

namespace Metrix.Infrastructure.Repositories;

public class HRDashboardRepository : IHRDashboardRepository
{
    private readonly AppDbContext _db;

    public HRDashboardRepository(AppDbContext db)
    {
        _db = db;
    }

    // ───────────────── INVITATIONS ─────────────────

    public async Task<Invitation> CreateInvitationAsync(Invitation invitation)
    {
        _db.Invitations.Add(invitation);
        await _db.SaveChangesAsync();
        return invitation;
    }

    public async Task<Invitation?> GetInvitationByTokenAsync(string token)
     => await _db.Invitations
                 .Include(x => x.CreatedByHR)
                 .AsNoTracking()
                 .FirstOrDefaultAsync(x => x.Token == token);

    public async Task<List<Invitation>> GetInvitationsByHRAsync(int hrId)
        => await _db.Invitations
                    .Include(x => x.CreatedByHR)
                    .Where(x => x.CreatedByHRId == hrId)
                    .OrderByDescending(x => x.Id)
                    .AsNoTracking()
                    .ToListAsync();

    public async Task<int> CountInvitationsByHRAsync(int hrId)
        => await _db.Invitations
                    .CountAsync(x => x.CreatedByHRId == hrId);

    // ───────────────── VISITORS ─────────────────

    public async Task<List<Visitor>> GetAllVisitorsByHRAsync(int hrId)
        => await _db.Visitors
                    .Include(x => x.Invitation)
                        .ThenInclude(i => i!.CreatedByHR)
                    .Include(x => x.ApprovedByHR)
                    .Where(x => x.Invitation!.CreatedByHRId == hrId)
                    .OrderByDescending(x => x.SubmittedAt)
                    .AsNoTracking()
                    .ToListAsync();

    public async Task<List<Visitor>> GetPendingVisitorsByHRAsync(int hrId)
        => await _db.Visitors
                    .Include(x => x.Invitation)
                        .ThenInclude(i => i!.CreatedByHR)
                    .Where(x => x.Invitation!.CreatedByHRId == hrId
                             && x.Status == VisitorStatus.Pending)
                    .OrderBy(x => x.SubmittedAt)
                    .AsNoTracking()
                    .ToListAsync();

    public async Task<List<Visitor>> GetRecentVisitorsByHRAsync(int hrId, int count = 5)
        => await _db.Visitors
                    .Include(x => x.Invitation)
                        .ThenInclude(i => i!.CreatedByHR)
                    .Where(x => x.Invitation!.CreatedByHRId == hrId)
                    .OrderByDescending(x => x.SubmittedAt)
                    .Take(count)
                    .AsNoTracking()
                    .ToListAsync();

    public async Task<Visitor?> GetVisitorByIdAsync(int id)
        => await _db.Visitors
                    .Include(x => x.Invitation)
                        .ThenInclude(i => i!.CreatedByHR)
                    .Include(x => x.ApprovedByHR)
                    .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<Visitor> UpdateVisitorAsync(Visitor visitor)
    {
        _db.Visitors.Update(visitor);
        await _db.SaveChangesAsync();
        return visitor;
    }

    // ── NEW: Visitor creation (called by VisitorService after registration) ───

    public async Task<Visitor> CreateVisitorAsync(Visitor visitor)
    {
        _db.Visitors.Add(visitor);
        await _db.SaveChangesAsync();
        return visitor;
    }

    // ── NEW: Invitation status update (Pending → Registered) ─────────────────

    public async Task<Invitation> UpdateInvitationAsync(Invitation invitation)
    {
        _db.Invitations.Update(invitation);
        await _db.SaveChangesAsync();
        return invitation;
    }

    // ───────────────── DASHBOARD COUNTS ─────────────────

    public async Task<int> CountPendingByHRAsync(int hrId)
        => await _db.Visitors
                    .CountAsync(x => x.Invitation!.CreatedByHRId == hrId
                                  && x.Status == VisitorStatus.Pending);

    public async Task<int> CountApprovedTodayByHRAsync(int hrId)
    {
        var today = DateTime.UtcNow.Date;
        return await _db.Visitors
                        .CountAsync(x => x.Invitation!.CreatedByHRId == hrId
                                      && x.Status == VisitorStatus.Approved
                                      && x.ApprovedAt.HasValue
                                      && x.ApprovedAt.Value.Date == today);
    }

    public async Task<int> CountTotalVisitorsByHRAsync(int hrId)
        => await _db.Visitors
                    .CountAsync(x => x.Invitation!.CreatedByHRId == hrId);

    public async Task<int> CountTodayVisitorsByHRAsync(int hrId)
    {
        var today = DateTime.UtcNow.Date;
        return await _db.Visitors
                        .CountAsync(x => x.Invitation!.CreatedByHRId == hrId
                                      && x.Invitation!.VisitDate.Date == today);
    }

    public async Task<int> CountAllApprovedByHRAsync(int hrId)
        => await _db.Visitors
                    .CountAsync(x => x.Invitation!.CreatedByHRId == hrId
                                  && x.Status == VisitorStatus.Approved);
}