using Metrix.API.Constants;
using Metrix.API.Handlers;
using Metrix.Application.DTOs.Admin;
using Metrix.Application.DTOs.Security;

namespace Metrix.API.Endpoints;

public static class AdminEndpoint
{
    public static void MapAdminEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Setup.CreateAdmin,
            async (CreateAdminHandler handler, RegisterAdminRequestDto request)
                => await handler.Handle(request))
            .WithName("CreateAdmin")
            .WithTags("Setup")
            .AllowAnonymous();

        app.MapPost(ApiRoutes.Admin.CreateSecurity,
            async (RegisterSecurityHandler handler,
                   RegisterSecurityRequestDto request)
                => await handler.RegisterSecurity(request))
            .RequireAuthorization("AdminOnly")
            .WithTags("Admin");
    }
}