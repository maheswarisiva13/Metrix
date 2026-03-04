using Metrix.Application.DTOs;
using Metrix.Application.DTOs.Invitation;
using Metrix.Application.Interfaces.Services;
using System.Security.Claims;

namespace Metrix.API.Handlers;

public class SendInvitationHandler
{
    private readonly IInvitationService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SendInvitationHandler(IInvitationService service, IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }



    public async Task<IResult> Handle(SendInvitationDto request)
    {
        var ctx = _httpContextAccessor.HttpContext;
        if (ctx == null)
            return Results.Unauthorized();

        // Get role from JWT
        var roleClaim = ctx.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        if (roleClaim == null || roleClaim.Value != "HR")
            return Results.Forbid();

        var hrIdClaim = ctx.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
        if (hrIdClaim == null)
            return Results.Unauthorized();

        var hrId = int.Parse(hrIdClaim.Value);

        await _service.SendInvitationAsync(
            request.VisitorName,
            request.VisitorEmail,
            request.Purpose,
            request.VisitDate,
            hrId
        );

        return Results.Ok(new { message = "Invitation sent successfully" });
    }
}