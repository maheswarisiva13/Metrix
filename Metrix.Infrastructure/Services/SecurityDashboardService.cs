// NEW FILE → Metrix.Infrastructure/Services/SecurityService.cs


using Metrix.Application.DTOs.Security;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;
using Metrix.Domain.Entities;

namespace Metrix.Infrastructure.Services;

public class SecurityDashboardService : ISecurityDashboardService
{
    private readonly ISecurityDashboardRepository _repo;

    public SecurityDashboardService(ISecurityDashboardRepository repo) => _repo = repo;

    // ── Dashboard ─────────────────────────────────────────────────────────────

    public async Task<SecurityDashboardDto> GetDashboardAsync(int securityId)
    {
        var todayVisitors = await _repo.CountTodayVisitorsAsync();
        var checkedIn = await _repo.CountCheckedInAsync();
        var checkedInToday = await _repo.CountCheckedInTodayAsync();
        var checkedOutToday = await _repo.CountCheckedOutTodayAsync();

        return new SecurityDashboardDto
        {
            TodayVisitors = todayVisitors,
            InsideNow = checkedIn,
            CheckedInToday = checkedInToday,
            CheckedOutToday = checkedOutToday,
            
        };
    }

    // ── Lookup ────────────────────────────────────────────────────────────────

    public async Task<SecurityVisitorDto?> LookupVisitorAsync(string registrationId)
    {
        var v = await _repo.GetVisitorByRegistrationIdAsync(registrationId.Trim().ToUpper());
        return v is null ? null : ToDto(v);
    }

    // ── Visitor lists ─────────────────────────────────────────────────────────

    public async Task<List<SecurityVisitorDto>> GetCheckedInVisitorsAsync()
        => (await _repo.GetCheckedInVisitorsAsync()).Select(ToDto).ToList();

    public async Task<List<SecurityVisitorDto>> GetTodayVisitorsAsync()
        => (await _repo.GetTodayVisitorsAsync()).Select(ToDto).ToList();

    public async Task<List<SecurityVisitorDto>> GetAllVisitorsAsync()
        => (await _repo.GetAllVisitorsAsync()).Select(ToDto).ToList();

    // ── Logs ──────────────────────────────────────────────────────────────────

    public async Task<List<VisitLogDto>> GetTodayLogsAsync()
    {
        var logs = await _repo.GetTodayLogsAsync();
        var result = new List<VisitLogDto>();

        foreach (var log in logs)
        {
            // Each VisitLog row = a check-in event
            result.Add(new VisitLogDto
            {
                Id = log.Id,
                VisitorName = log.Visitor?.Name ?? string.Empty,
                RegistrationId = log.Visitor?.RegistrationId,
                Purpose = log.Visitor?.Invitation?.Purpose ?? string.Empty,
                EventType = "CheckIn",
                EntryTime = log.EntryTime,
                ExitTime = log.ExitTime,
                VerifiedBy = log.VerifiedBySecurity?.Name ?? string.Empty,
            });

            // If already checked out, add a separate CheckOut event row
            if (log.ExitTime.HasValue)
            {
                result.Add(new VisitLogDto
                {
                    Id = log.Id * -1, // negative so FE key stays unique
                    VisitorName = log.Visitor?.Name ?? string.Empty,
                    RegistrationId = log.Visitor?.RegistrationId,
                    Purpose = log.Visitor?.Invitation?.Purpose ?? string.Empty,
                    EventType = "CheckOut",
                    EntryTime = log.EntryTime,
                    ExitTime = log.ExitTime,
                    VerifiedBy = log.VerifiedBySecurity?.Name ?? string.Empty,
                });
            }
        }

        // Sort by whichever time is most recent first
        return result
            .OrderByDescending(r => r.EventType == "CheckOut" ? r.ExitTime ?? r.EntryTime : r.EntryTime)
            .ToList();
    }

    // ── Check In ─────────────────────────────────────────────────────────────

    public async Task<CheckInOutDto> CheckInAsync(int visitorId, int securityId)
    {
        var visitor = await _repo.GetVisitorByIdAsync(visitorId)
            ?? throw new KeyNotFoundException("Visitor not found.");

        if (visitor.Status.ToString() != "Approved")
            throw new InvalidOperationException("Only approved visitors can be checked in.");

        // Make sure not already inside
        var openLog = await _repo.GetOpenLogAsync(visitorId);
        if (openLog is not null)
            throw new InvalidOperationException("Visitor is already checked in.");

        var log = new VisitLog
        {
            VisitorId = visitorId,
            EntryTime = DateTime.UtcNow,
            VerifiedBySecurityId = securityId,
        };
        await _repo.CreateLogAsync(log);

        return new CheckInOutDto
        {
            Success = true,
            Message = $"{visitor.Name} has been checked in.",
            Time = log.EntryTime,
        };
    }

    // ── Check Out ─────────────────────────────────────────────────────────────

    public async Task<CheckInOutDto> CheckOutAsync(int visitorId, int securityId)
    {
        var openLog = await _repo.GetOpenLogAsync(visitorId)
            ?? throw new InvalidOperationException("Visitor is not currently checked in.");

        openLog.ExitTime = DateTime.UtcNow;
        await _repo.UpdateLogAsync(openLog);

        var visitor = await _repo.GetVisitorByIdAsync(visitorId);

        return new CheckInOutDto
        {
            Success = true,
            Message = $"{visitor?.Name ?? "Visitor"} has been checked out.",
            Time = openLog.ExitTime.Value,
        };
    }
    public async Task<List<SecurityVisitorDto>> GetVisitorHistoryAsync()
    {
        var visitors = await _repo.GetVisitorHistoryAsync();
        return visitors.Select(ToDto).ToList();
    }

    // ── Mapper ────────────────────────────────────────────────────────────────

    private static SecurityVisitorDto ToDto(Visitor v)
    {
        var openLog = v.VisitLogs.FirstOrDefault(l => l.ExitTime == null);
        var lastLog = v.VisitLogs.OrderByDescending(l => l.EntryTime).FirstOrDefault();
        var checkOut = v.VisitLogs.Where(l => l.ExitTime.HasValue)
                                  .OrderByDescending(l => l.ExitTime).FirstOrDefault();

        return new SecurityVisitorDto
        {
            Id = v.Id,
            Name = v.Name,
            Email = v.Email,
            Phone = v.Phone,
            Purpose = v.Invitation?.Purpose ?? string.Empty,
            VisitDate = v.Invitation?.VisitDate ?? default,
            IdProofType = v.IDProofType.ToString(),
            IdProofNumber = v.IDProofNumber,
            RegistrationId = v.RegistrationId,
            Status = v.Status.ToString(),
            HrName = v.Invitation?.CreatedByHR?.Name ?? string.Empty,
            SubmittedAt = v.SubmittedAt,
            ApprovedAt = v.ApprovedAt,
            ApprovedByHR = v.ApprovedByHR?.Name,
            CheckedInAt = openLog?.EntryTime ?? lastLog?.EntryTime,
            CheckedOutAt = checkOut?.ExitTime,
        };
    }

}