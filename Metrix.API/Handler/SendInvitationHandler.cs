using Metrix.Application.DTOs;
using Metrix.Application.DTOs.Invitation;
using Metrix.Application.Interfaces.Services;

namespace Metrix.API.Handlers;

public class SendInvitationHandler
{
    private readonly IInvitationService _service;

    public SendInvitationHandler(IInvitationService service)
    {
        _service = service;
    }

    public async Task<IResult> Handle(SendInvitationDto request)
    {
        await _service.SendInvitationAsync(
            request.VisitorName,
            request.VisitorEmail,
            request.Purpose,
            request.VisitDate,
            request.CreatedByHrId);

        return Results.Ok(new
        {
            message = "Invitation sent successfully"
        });
    }
}
