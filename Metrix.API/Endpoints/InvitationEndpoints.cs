using Metrix.API.Constants;
using Metrix.API.Handlers;
using Metrix.Application.DTOs;
using Metrix.Application.DTOs.Invitation;

namespace Metrix.API.Endpoints;

public static class InvitationEndpoints
{
    public static void MapInvitationEndpoints(this IEndpointRouteBuilder app)
    {
        // POST - Send invitation
        app.MapPost(ApiRoutes.Invitation.Send,
    async (SendInvitationHandler handler, SendInvitationDto request)
        => await handler.Handle(request))
    .WithName("SendInvitation")
    .WithTags("Invitation")
    .RequireAuthorization();


    }
}

