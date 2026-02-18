using Metrix.Application.DTOs.Auth;
using Metrix.Application.Interfaces.Services;

public class LoginHandler
{
    private readonly IAuthService _authService;

    public LoginHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IResult> HandleAsync(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        if (result == null)
            return Results.Unauthorized();

        return Results.Ok(result);
    }
}
