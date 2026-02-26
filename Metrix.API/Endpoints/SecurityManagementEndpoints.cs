using Metrix.API.Constants;
using Metrix.API.Handlers;
using Metrix.Application.DTOs.Security;

namespace Metrix.API.Endpoints;

public static class SecurityManagementEndpoints
{
    public static void MapSecurityManagementEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Security.Root,
            async (RegisterSecurityHandler handler,
                   RegisterSecurityRequestDto request)
                => await handler.RegisterSecurity(request))
            .RequireAuthorization("AdminOnly")
            .WithTags("Admin");
    }
}
