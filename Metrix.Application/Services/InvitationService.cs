using Metrix.Application.DTOs.Invitation;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;
using Metrix.Domain.Entities;
using Metrix.Domain.Enums;

namespace Metrix.Application.Services;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _repository;
    private readonly IEmailService _emailService;

    public InvitationService(
        IInvitationRepository repository,
        IEmailService emailService)
    {
        _repository = repository;
        _emailService = emailService;
    }

    /// <summary>
    /// Sends an invitation to a visitor and returns the registration link.
    /// </summary>
    public async Task<InvitationResponseDto> SendInvitationAsync(
        string visitorName,
        string visitorEmail,
        string purpose,
        DateTime visitDate,
        int createdByHrId)
    {
        // Generate a unique token for registration
        var token = Guid.NewGuid().ToString();

        // Create invitation entity
        var invitation = new Invitation
        {
            VisitorName = visitorName,
            VisitorEmail = visitorEmail,
            Purpose = purpose,
            VisitDate = visitDate.ToUniversalTime(),
            Token = token,
            Status = InvitationStatus.Pending,
            CreatedByHRId = createdByHrId
        };

        // Save to database
        await _repository.AddAsync(invitation);
        await _repository.SaveChangesAsync();

        // Build registration link
        var registrationLink = $"http://localhost:5173/register?token={token}";

        // Send email to visitor
        var emailBody = $@"
            <h2>Invitation</h2>
            <p>Hello {visitorName},</p>
            <p>Please click the link below to complete your registration:</p>
            <a href='{registrationLink}'>Complete Registration</a>
        ";

        await _emailService.SendAsync(
            visitorEmail,
            visitorName,               // display name in email
            "Complete Your Registration",
            emailBody
        );

        // Return response to frontend
        return new InvitationResponseDto
        {
            InviteLink = registrationLink
        };
    }
}