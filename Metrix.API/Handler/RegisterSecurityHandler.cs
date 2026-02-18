using Metrix.API.Constants;
using Metrix.Application.DTOs.Security;
using Metrix.Application.Interfaces.Services;

namespace Metrix.API.Handlers;

public class RegisterSecurityHandler
{
    private readonly ISecurityService _securityService;

    public RegisterSecurityHandler(ISecurityService securityService)
    {
        _securityService = securityService;
    }

    public async Task<IResult> RegisterSecurity(RegisterSecurityRequestDto request)
    {
        try
        {
            var id = await _securityService.RegisterSecurityAsync(request);

            return Results.Created(
                $"{ApiRoutes.Security.Root}/{id}",
                new { id }
            );
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
