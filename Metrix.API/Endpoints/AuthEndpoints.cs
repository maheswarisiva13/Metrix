using Metrix.Application.DTOs.Auth;
using Metrix.Application.Interfaces.Services;

namespace Metrix.API.Endpoints;

public static class AuthEndpoints
{


    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/auth/login", async (
            LoginRequestDto request,
            IAuthService authService) =>
        {
            var result = await authService.LoginAsync(request);

            if (result == null)
                return Results.Unauthorized();

            return Results.Ok(result);
        });
    }


}


