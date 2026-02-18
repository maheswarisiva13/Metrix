using Metrix.API.Constants;
using Metrix.Application.DTOs.HR;

namespace Metrix.API.Endpoints;

public static class HrManagementEndpoints
{
    public static void MapHrManagementEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost(ApiRoutes.Hr.Root,
     async (RegisterHrHandler handler, RegisterHrRequestDto request)
         => await handler.RegisterHr(request))
     .WithName("CreateHrUser")
     .WithTags("HR Management")
     .AllowAnonymous();

    }
}
