using Metrix.Application.DTOs.Visitor;

namespace Metrix.Application.Interfaces.Services;

/// <summary>
/// Public visitor self-registration service.
/// No auth required — uses invitation token instead.
///
/// Add to: Metrix.Application/Interfaces/Services/IVisitorService.cs
/// </summary>
public interface IVisitorService
{
    /// <summary>
    /// Validates the token and returns invitation details so the
    /// registration form can be pre-filled.
    /// Throws KeyNotFoundException if token doesn't exist.
    /// Throws InvalidOperationException if already registered/approved/rejected.
    /// </summary>
    Task<InviteDetailsDto> GetInviteDetailsAsync(string token);

    /// <summary>
    /// Creates a Visitor row linked to the Invitation, sets status to Pending,
    /// marks Invitation.Status = Registered, then sends a notification email
    /// to the HR user so they see the pending visitor on their dashboard.
    /// </summary>
    Task<VisitorRegisterResponseDto> RegisterAsync(VisitorRegisterRequest request);
}