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

    public async Task SendInvitationAsync(
        string visitorName,
        string visitorEmail,
        string purpose,
        DateTime visitDate,
        int createdByHrId
        )
    {
        var token = Guid.NewGuid().ToString();

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

        await _repository.AddAsync(invitation);
        await _repository.SaveChangesAsync();

        var registrationLink =
  $"http://localhost:5173/register?token={token}";


        var body = $@"
            <h2>Invitation</h2>
            <p>Hello {visitorName},</p>
            <p>Please click below to complete registration:</p>
            <a href='{registrationLink}'>Complete Registration</a>
        ";

        await _emailService.SendAsync(
    visitorEmail,
    visitorName,       // display name in email
    "Complete Your Registration",
    body
);
    }
}
