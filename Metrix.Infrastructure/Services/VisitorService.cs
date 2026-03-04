using Metrix.Application.DTOs.Visitor;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;
using Metrix.Domain.Entities;
using Metrix.Domain.Enums;

namespace Metrix.Infrastructure.Services;

/// <summary>
/// Add to: Metrix.Infrastructure/Services/VisitorService.cs
///
/// Handles public visitor self-registration flow:
///   1. Visitor opens link → GET /api/visitor/invite?token=XYZ
///   2. Visitor fills form → POST /api/visitor/register
///   3. Sends HR notification email so they see pending visitor
/// </summary>
public class VisitorService : IVisitorService
{
    private readonly IHRDashboardRepository _repo;
    private readonly IEmailService _email;

    public VisitorService(IHRDashboardRepository repo, IEmailService email)
    {
        _repo = repo;
        _email = email;
    }

    // ════════════════════════════════════════════════════════
    //  STEP 1 — Validate token, return pre-fill data
    // ════════════════════════════════════════════════════════

    public async Task<InviteDetailsDto> GetInviteDetailsAsync(string token)
    {
        var invitation = await _repo.GetInvitationByTokenAsync(token)
            ?? throw new KeyNotFoundException("This invitation link is invalid or has expired.");

        // Tell the frontend what state this invitation is in
        var statusLabel = invitation.Status switch
        {
            InvitationStatus.Pending => "Pending",
            InvitationStatus.Registered => "Registered",
            InvitationStatus.Approved => "Approved",
            InvitationStatus.Rejected => "Rejected",
            _ => "Unknown",
        };

        return new InviteDetailsDto
        {
            VisitorName = invitation.VisitorName,
            VisitorEmail = invitation.VisitorEmail,
            Purpose = invitation.Purpose,
            VisitDate = invitation.VisitDate,
            HRName = invitation.CreatedByHR?.Name ?? "HR Team",
            Status = statusLabel,
        };
    }

    // ════════════════════════════════════════════════════════
    //  STEP 2 — Register visitor
    // ════════════════════════════════════════════════════════

    public async Task<VisitorRegisterResponseDto> RegisterAsync(VisitorRegisterRequest request)
    {
        // 1. Load invitation (with HR navigation)
        var invitation = await _repo.GetInvitationByTokenAsync(request.Token)
            ?? throw new KeyNotFoundException("Invalid invitation token.");

        // 2. Guard: only Pending invitations can register
        if (invitation.Status != InvitationStatus.Pending)
        {
            var reason = invitation.Status switch
            {
                InvitationStatus.Registered => "You have already submitted your registration.",
                InvitationStatus.Approved => "Your visit has already been approved.",
                InvitationStatus.Rejected => "This invitation has been rejected.",
                _ => "This invitation is no longer valid.",
            };
            throw new InvalidOperationException(reason);
        }

        // 3. Guard: one registration per invitation
       // if (invitation.Visitor is not null)
          //  throw new InvalidOperationException("A registration already exists for this invitation.");

        // 4. Parse ID proof type
        if (!Enum.TryParse<IDProofType>(request.IdProofType, true, out var idType))
            throw new ArgumentException($"Invalid ID proof type: {request.IdProofType}. " +
                "Valid values: Aadhaar, Passport, DrivingLicense, VoterID");

        // 5. Create Visitor row
        var visitor = new Visitor
        {
            InvitationId = invitation.Id,
            Name = request.Name.Trim(),
            Email = request.Email.Trim().ToLower(),
            Phone = request.Phone.Trim(),
            IDProofType = idType,
            IDProofNumber = request.IdProofNumber.Trim(),
            Status = VisitorStatus.Pending,
            SubmittedAt = DateTime.UtcNow,
        };

        await _repo.CreateVisitorAsync(visitor);

        // 6. Mark Invitation as Registered
        invitation.Status = InvitationStatus.Registered;
        await _repo.UpdateInvitationAsync(invitation);

        // 7. Send notification email to HR
        //    This is what makes the visitor appear in HR's Pending Approvals
        if (invitation.CreatedByHR is not null)
        {
            await _email.SendVisitorRegisteredNotificationAsync(
                hrEmail: invitation.CreatedByHR.Email,
                hrName: invitation.CreatedByHR.Name,
                visitorName: visitor.Name,
                purpose: invitation.Purpose,
                visitDate: invitation.VisitDate,
                dashboardUrl: "http://localhost:5173/hr/pending"  // configurable
            );
        }

        return new VisitorRegisterResponseDto
        {
            Success = true,
            Message = "Registration submitted. HR will review and contact you with a decision.",
            VisitorName = visitor.Name,
        };
    }
}