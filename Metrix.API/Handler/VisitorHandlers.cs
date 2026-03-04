
using Metrix.Application.DTOs.Visitor;
using Metrix.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Metrix.API.Handlers;

/// <summary>
/// File: Metrix.API/Handlers/VisitorHandlers.cs  (NEW FILE)
/// </summary>
public static class VisitorHandlers
{
    /// <summary>
    /// GET /api/visitor/invite?token=XXXXXXXXXXXXXXXX
    ///
    /// Called immediately when visitor opens email link.
    /// Returns invitation details to pre-fill the registration form.
    /// Returns 404 if token invalid, 400 if already registered.
    /// </summary>
    public static async Task<IResult> GetInviteDetailsAsync(
        [FromQuery] string token,
        IVisitorService visitorService)
    {
        if (string.IsNullOrWhiteSpace(token))
            return Results.BadRequest(new { message = "Token is required." });

        var dto = await visitorService.GetInviteDetailsAsync(token);
        return Results.Ok(dto);
    }

    /// <summary>
    /// POST /api/visitor/register
    ///
    /// Visitor submits their details. Creates Visitor row, marks Invitation
    /// as Registered, emails HR team so they see it in Pending Approvals.
    /// </summary>
    public static async Task<IResult> RegisterVisitorAsync(
        [FromBody] VisitorRegisterRequest request,
        IVisitorService visitorService)
    {
        var result = await visitorService.RegisterAsync(request);
        return Results.Ok(result);
    }
}