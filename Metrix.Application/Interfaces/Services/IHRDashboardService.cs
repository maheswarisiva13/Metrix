using Metrix.Application.DTOs.HR;


namespace Metrix.Application.Interfaces.Services;

/// <summary>
/// Handles HR Dashboard operations.
/// Authentication methods remain in IHRService.
/// </summary>
public interface IHRDashboardService
{
    // ── Dashboard ───────────────────────────────────────────────
    Task<HRDashboardStatsDto> GetDashboardStatsAsync(int hrId);

    // ── Visitors ─────────────────────────────────────────────────
    Task<List<VisitorDto>> GetAllVisitorsAsync(int hrId);
    Task<List<VisitorDto>> GetPendingVisitorsAsync(int hrId);
    Task<List<VisitorDto>> GetRecentVisitorsAsync(int hrId);
    Task<ApproveVisitorDto> ApproveVisitorAsync(int visitorId, int hrId);
    Task<RejectVisitorDto> RejectVisitorAsync(int visitorId, int hrId);

    // ── Invitations ──────────────────────────────────────────────
    Task<SendInvitationDto> SendInvitationAsync(int hrId, SendInvitationRequest request);
    Task<List<InvitationDto>> GetInvitationsAsync(int hrId);
}