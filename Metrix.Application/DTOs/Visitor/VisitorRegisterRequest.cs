using System.ComponentModel.DataAnnotations;

namespace Metrix.Application.DTOs.Visitor;

// ── Step 1: GET /api/visitor/invite?token=XYZ ──────────────────────────────────
// No request body — token comes from query string

// ── Step 2: POST /api/visitor/register ────────────────────────────────────────

/// <summary>
/// Visitor submits this from the public registration page.
/// The token ties this submission to the correct Invitation row.
/// NO auth token needed — this is a public endpoint.
/// </summary>
public class VisitorRegisterRequest
{
    /// <summary>Invitation token from the email link: /register?token=XXXXXXXXXXXXXXXX</summary>
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(20)]
    public string Phone { get; set; } = string.Empty;

    /// <summary>Aadhaar | Passport | DrivingLicense | VoterID</summary>
    [Required]
    public string IdProofType { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string IdProofNumber { get; set; } = string.Empty;

    public string? Notes { get; set; }
}