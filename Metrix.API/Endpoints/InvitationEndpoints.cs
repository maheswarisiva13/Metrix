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
            async (SendInvitationHandler handler,
                   SendInvitationDto request)
                => await handler.Handle(request))
            .WithName("SendInvitation")
            .WithTags("Invitation")
            .AllowAnonymous();


        // GET - Registration page (Temporary test page)
        app.MapGet("/api/invitations/register", (string token) =>
        {
            var html = $@"
                <html>
                    <body>
                        <h2>Registration Page</h2>
                        <p>Your token: {token}</p>
                        <p>Here we will build registration form later.</p>
                    </body>
                </html>";

            return Results.Content(html, "text/html");
        })
        .WithName("InvitationRegistrationPage")
        .WithTags("Invitation")
        .AllowAnonymous();
    }
}

