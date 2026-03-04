namespace Metrix.Application.DTOs.HR;

// ── Dashboard stats ────────────────────────────────────────────────────────────

/// <summary>
/// GET /api/hr/dashboard
/// Frontend reads: totalInvitations, pendingApprovals, approvedToday, totalVisitors, todayVisitors
/// </summary>
public class HRDashboardStatsDto
{
    public int TotalInvitations { get; set; }
    public int PendingApprovals { get; set; }
    public int ApprovedToday { get; set; }
    public int TotalVisitors { get; set; }
    public int TodayVisitors { get; set; }
}

// ── Visitor ────────────────────────────────────────────────────────────────────

/// <summary>
/// Returned by GET /api/hr/visitors, /pending, /recent
/// Frontend fields (must match exactly):
///   id, name, email, phone, purpose, visitDate,
///   idProofType, idProofNumber, photoPath,
///   registrationId, status, submittedAt, approvedAt
/// </summary>
public class VisitorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;  // from Invitation
    public DateTime VisitDate { get; set; }                   // from Invitation
    public string IdProofType { get; set; } = string.Empty;  // enum → string
    public string IdProofNumber { get; set; } = string.Empty;
    public string PhotoPath { get; set; } = string.Empty;
    public string? RegistrationId { get; set; }

    /// <summary>"Pending" | "Approved" | "Rejected"</summary>
    public string Status { get; set; } = string.Empty;

    public DateTime SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovedByHR { get; set; }  // HR name who approved
    public string HRName { get; set; } = string.Empty; // invitation creator
}

// ── Approve ────────────────────────────────────────────────────────────────────

/// <summary>
/// POST /api/hr/visitors/{id}/approve
/// Frontend reads: registrationId
/// </summary>
public class ApproveVisitorDto
{
    public string RegistrationId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

// ── Reject ─────────────────────────────────────────────────────────────────────

/// <summary>
/// POST /api/hr/visitors/{id}/reject
/// Frontend reads: success
/// </summary>
public class RejectVisitorDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

// ── Invitation ─────────────────────────────────────────────────────────────────

/// <summary>
/// GET /api/invitations
/// </summary>
public class InvitationDto
{
    public int Id { get; set; }
    public string VisitorName { get; set; } = string.Empty;
    public string VisitorEmail { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string Token { get; set; } = string.Empty;

    /// <summary>"Pending" | "Registered" | "Approved" | "Rejected"</summary>
    public string Status { get; set; } = string.Empty;

    public DateTime SentAt { get; set; }
    public string HRName { get; set; } = string.Empty;
}

/// <summary>
/// POST /api/invitations/send
/// Frontend reads: token, inviteLink
/// </summary>
public class SendInvitationDto
{
    public string Token { get; set; } = string.Empty;
    public string InviteLink { get; set; } = string.Empty;
}