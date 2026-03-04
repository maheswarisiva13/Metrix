using Metrix.Domain.Entities;

namespace Metrix.Application.Interfaces.Repositories;

/// <summary>
/// Handles HR Dashboard, Invitations, Visitors & Stats
/// Authentication methods remain in IHRRepository.
/// </summary>
public interface IHRDashboardRepository
{
   
    // ── Invitations ───────────────────────────────────────────────
    Task<Invitation> CreateInvitationAsync(Invitation invitation);
    Task<Invitation?> GetInvitationByTokenAsync(string token);
    Task<List<Invitation>> GetInvitationsByHRAsync(int hrId);
    Task<int> CountInvitationsByHRAsync(int hrId);

    // ── Visitors ───────────────────────────────────────────────────
    Task<List<Visitor>> GetAllVisitorsByHRAsync(int hrId);
    Task<List<Visitor>> GetPendingVisitorsByHRAsync(int hrId);
    Task<List<Visitor>> GetRecentVisitorsByHRAsync(int hrId, int count = 5);
    Task<Visitor?> GetVisitorByIdAsync(int id);
    Task<Visitor> UpdateVisitorAsync(Visitor visitor);

    // ── Dashboard Stats ────────────────────────────────────────────
    Task<int> CountPendingByHRAsync(int hrId);
    Task<int> CountApprovedTodayByHRAsync(int hrId);
    Task<int> CountTotalVisitorsByHRAsync(int hrId);
    Task<int> CountTodayVisitorsByHRAsync(int hrId);
    Task<int> CountAllApprovedByHRAsync(int hrId);

    // ── NEW: needed by VisitorService ────────────────────────────────────────

    /// <summary>Creates a new Visitor row after self-registration.</summary>
    Task<Visitor> CreateVisitorAsync(Visitor visitor);

    /// <summary>Updates Invitation.Status to Registered after visitor submits form.</summary>
    Task<Invitation> UpdateInvitationAsync(Invitation invitation);
}