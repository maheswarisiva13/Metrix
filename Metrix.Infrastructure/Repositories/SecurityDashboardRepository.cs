using Microsoft.EntityFrameworkCore;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Domain.Entities;
using Metrix.Domain.Enums;
using Metrix.Infrastructure.Persistence;

namespace Metrix.Infrastructure.Repositories;

public class SecurityDashboardRepository : ISecurityDashboardRepository
{
    private readonly AppDbContext _db;

    public SecurityDashboardRepository(AppDbContext db) => _db = db;

    // ── Auth ──────────────────────────────────────────────────────────────────

    public async Task<SecurityUser?> GetByEmailAsync(string email)
        => await _db.SecurityUsers
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Email == email.ToLower() && x.IsActive);

    // ── Visitors ──────────────────────────────────────────────────────────────

    public async Task<Visitor?> GetVisitorByRegistrationIdAsync(string registrationId)
        => await _db.Visitors
                    .Include(v => v.Invitation).ThenInclude(i => i!.CreatedByHR)
                    .Include(v => v.ApprovedByHR)
                    .Include(v => v.VisitLogs)
                    .FirstOrDefaultAsync(v => v.RegistrationId == registrationId);

    public async Task<Visitor?> GetVisitorByIdAsync(int id)
        => await _db.Visitors
                    .Include(v => v.Invitation).ThenInclude(i => i!.CreatedByHR)
                    .Include(v => v.ApprovedByHR)
                    .Include(v => v.VisitLogs)
                    .FirstOrDefaultAsync(v => v.Id == id);

    // ── Visitors currently inside ─────────────────────────────────────────────
    public async Task<List<Visitor>> GetCheckedInVisitorsAsync()
    {
        // Visitors with open logs (no ExitTime)
        return await _db.Visitors
            .Include(v => v.Invitation).ThenInclude(i => i!.CreatedByHR)
            .Include(v => v.VisitLogs)
            .Where(v => v.VisitLogs.Any(l => l.ExitTime == null))
            .OrderByDescending(v => v.VisitLogs
                                     .Where(l => l.ExitTime == null)
                                     .Max(l => l.EntryTime))
            .ToListAsync();
    }

    // ── Today Visitors ────────────────────────────────────────────────────────
    public async Task<List<Visitor>> GetTodayVisitorsAsync()
    {
        var todayLocal = DateTime.Today;
        var tomorrowLocal = todayLocal.AddDays(1);

        var utcStart = TimeZoneInfo.ConvertTimeToUtc(todayLocal);
        var utcEnd = TimeZoneInfo.ConvertTimeToUtc(tomorrowLocal);

        return await _db.Visitors
            .Include(v => v.Invitation).ThenInclude(i => i!.CreatedByHR)
            .Include(v => v.VisitLogs)
            .Where(v =>
                v.Invitation != null &&
                v.Invitation.VisitDate >= utcStart &&
                v.Invitation.VisitDate < utcEnd &&
                v.Status == VisitorStatus.Approved)
            .OrderBy(v => v.Name)
            .ToListAsync();
    }

    public async Task<List<Visitor>> GetAllVisitorsAsync()
        => await _db.Visitors
                    .Include(v => v.Invitation).ThenInclude(i => i!.CreatedByHR)
                    .Include(v => v.ApprovedByHR)
                    .Include(v => v.VisitLogs)
                    .OrderByDescending(v => v.SubmittedAt)
                    .ToListAsync();

    public async Task UpdateVisitorAsync(Visitor visitor)
    {
        _db.Visitors.Update(visitor);
        await _db.SaveChangesAsync();
    }

    // ── Visit Logs ────────────────────────────────────────────────────────────

    public async Task<VisitLog?> GetOpenLogAsync(int visitorId)
        => await _db.VisitLogs
                    .Include(l => l.VerifiedBySecurity)
                    .FirstOrDefaultAsync(l => l.VisitorId == visitorId && l.ExitTime == null);

    public async Task<VisitLog> CreateLogAsync(VisitLog log)
    {
        _db.VisitLogs.Add(log);
        await _db.SaveChangesAsync();
        return log;
    }

    public async Task UpdateLogAsync(VisitLog log)
    {
        _db.VisitLogs.Update(log);
        await _db.SaveChangesAsync();
    }

    public async Task<List<VisitLog>> GetTodayLogsAsync()
    {
        var todayLocal = DateTime.Today;
        var tomorrowLocal = todayLocal.AddDays(1);

        var utcStart = TimeZoneInfo.ConvertTimeToUtc(todayLocal);
        var utcEnd = TimeZoneInfo.ConvertTimeToUtc(tomorrowLocal);

        return await _db.VisitLogs
            .Include(l => l.Visitor).ThenInclude(v => v!.Invitation)
            .Include(l => l.VerifiedBySecurity)
            .Where(l =>
                (l.EntryTime >= utcStart && l.EntryTime < utcEnd) ||
                (l.ExitTime.HasValue &&
                 l.ExitTime.Value >= utcStart &&
                 l.ExitTime.Value < utcEnd))
            .OrderByDescending(l => l.EntryTime)
            .ToListAsync();
    }

    // ── Dashboard Stats ───────────────────────────────────────────────────────

    public async Task<int> CountTodayVisitorsAsync()
    {
        var todayLocal = DateTime.Today;
        var tomorrowLocal = todayLocal.AddDays(1);

        var utcStart = TimeZoneInfo.ConvertTimeToUtc(todayLocal);
        var utcEnd = TimeZoneInfo.ConvertTimeToUtc(tomorrowLocal);

        return await _db.Visitors
            .Include(v => v.Invitation)
            .CountAsync(v =>
                v.Invitation != null &&
                v.Invitation.VisitDate >= utcStart &&
                v.Invitation.VisitDate < utcEnd &&
                v.Status == VisitorStatus.Approved);
    }

    public async Task<int> CountCheckedInAsync()
        => await _db.VisitLogs.CountAsync(l => l.ExitTime == null);

    public async Task<int> CountCheckedInTodayAsync()
    {
        var todayLocal = DateTime.Today;
        var tomorrowLocal = todayLocal.AddDays(1);

        var utcStart = TimeZoneInfo.ConvertTimeToUtc(todayLocal);
        var utcEnd = TimeZoneInfo.ConvertTimeToUtc(tomorrowLocal);

        return await _db.VisitLogs
            .CountAsync(l => l.EntryTime >= utcStart && l.EntryTime < utcEnd);
    }

    public async Task<int> CountCheckedOutTodayAsync()
    {
        var todayLocal = DateTime.Today;
        var tomorrowLocal = todayLocal.AddDays(1);

        var utcStart = TimeZoneInfo.ConvertTimeToUtc(todayLocal);
        var utcEnd = TimeZoneInfo.ConvertTimeToUtc(tomorrowLocal);

        return await _db.VisitLogs
            .CountAsync(l =>
                l.ExitTime.HasValue &&
                l.ExitTime.Value >= utcStart &&
                l.ExitTime.Value < utcEnd);
    }
    public async Task<List<Visitor>> GetVisitorHistoryAsync()
    {
        return await _db.Visitors
            .AsNoTracking()
            .Include(v => v.Invitation)
                .ThenInclude(i => i.CreatedByHR)
            .Include(v => v.ApprovedByHR)
            .Include(v => v.VisitLogs)
            .OrderByDescending(v => v.SubmittedAt)
            .ToListAsync();
    }

}