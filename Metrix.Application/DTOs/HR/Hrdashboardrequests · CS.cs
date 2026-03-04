using System.ComponentModel.DataAnnotations;

namespace Metrix.Application.DTOs.HR;

// ── Invitation ─────────────────────────────────────────────────────────────────

public class SendInvitationRequest
{
    [Required, MaxLength(100)]
    public string VisitorName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string VisitorEmail { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Purpose { get; set; } = string.Empty;

    [Required]
    public DateTime VisitDate { get; set; }

    public string? Notes { get; set; }
}