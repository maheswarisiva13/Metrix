namespace Metrix.Application.DTOs.Visitor;

// ── Response: GET /api/visitor/invite?token=XYZ ───────────────────────────────

/// <summary>
/// Returned when the visitor opens their invite link.
/// Pre-fills name, email, purpose, visitDate on the registration form.
/// </summary>
public class InviteDetailsDto
{
    public string VisitorName { get; set; } = string.Empty;
    public string VisitorEmail { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string HRName { get; set; } = string.Empty;

    /// <summary>
    /// "Pending"    → visitor can register
    /// "Registered" → already submitted, tell them not to re-submit
    /// "Approved"   → HR already approved
    /// "Rejected"   → invitation was rejected
    /// "Expired"    → token expired (if you add expiry logic)
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

// ── Response: POST /api/visitor/register ──────────────────────────────────────

/// <summary>
/// Returned after successful self-registration.
/// HR will now see this visitor in Pending Approvals.
/// </summary>
public class VisitorRegisterResponseDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Visitor's name echoed back for the success screen.
    /// </summary>
    public string VisitorName { get; set; } = string.Empty;
}
