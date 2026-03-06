using Metrix.Domain.Entities;

namespace Metrix.Application.Interfaces.Repositories;

public interface IAdminDashboardRepository
{
    // ── Security users ────────────────────────────────────────────────────────
    Task<List<SecurityUser>> GetAllSecurityUsersAsync();
    Task<bool> SecurityEmailExistsAsync(string email);
    Task<SecurityUser> CreateSecurityUserAsync(SecurityUser user);
    Task DeactivateSecurityUserAsync(int id);

    // ── Visitors ──────────────────────────────────────────────────────────────
    Task<List<Visitor>> GetAllVisitorsAsync();

    // ── Stats ─────────────────────────────────────────────────────────────────
    Task<int> CountSecurityUsersAsync();
    Task<int> CountActiveSecurityUsersAsync();
    Task<int> CountTotalVisitorsAsync();
    //Task<int> CountVisitorsByStatusAsync(string status);
    Task<int> CountCheckedInTodayAsync();
    Task<int> CountCheckedOutTodayAsync();
}