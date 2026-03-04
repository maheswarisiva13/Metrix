using Metrix.Application.DTOs.HR;
using Metrix.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Metrix.API.Handlers;

public static class HRDashboardHandlers
{
    // ── Dashboard ─────────────────────────────────────────────────────────────

    public static async Task<IResult> GetDashboardStatsAsync(
        ClaimsPrincipal principal,
        IHRDashboardService hrService)
    {
        var hrId = GetHRId(principal);
        var dto = await hrService.GetDashboardStatsAsync(hrId);
        return Results.Ok(dto);
    }

    // ── Visitors ──────────────────────────────────────────────────────────────

    public static async Task<IResult> GetAllVisitorsAsync(
        ClaimsPrincipal principal,
        IHRDashboardService hrService)
    {
        var hrId = GetHRId(principal);
        var list = await hrService.GetAllVisitorsAsync(hrId);
        return Results.Ok(list);
    }

    public static async Task<IResult> GetPendingVisitorsAsync(
        ClaimsPrincipal principal,
        IHRDashboardService hrService)
    {
        var hrId = GetHRId(principal);
        var list = await hrService.GetPendingVisitorsAsync(hrId);
        return Results.Ok(list);
    }

    public static async Task<IResult> GetRecentVisitorsAsync(
        ClaimsPrincipal principal,
        IHRDashboardService hrService)
    {
        var hrId = GetHRId(principal);
        var list = await hrService.GetRecentVisitorsAsync(hrId);
        return Results.Ok(list);
    }

    public static async Task<IResult> ApproveVisitorAsync(
        int id,
        ClaimsPrincipal principal,
        IHRDashboardService hrService)
    {
        var hrId = GetHRId(principal);
        var dto = await hrService.ApproveVisitorAsync(id, hrId);
        return Results.Ok(dto);
    }

    public static async Task<IResult> RejectVisitorAsync(
        int id,
        ClaimsPrincipal principal,
        IHRDashboardService hrService)
    {
        var hrId = GetHRId(principal);
        var dto = await hrService.RejectVisitorAsync(id, hrId);
        return Results.Ok(dto);
    }

    // ── Invitations ───────────────────────────────────────────────────────────

    public static async Task<IResult> SendInvitationAsync(
        [FromBody] SendInvitationRequest request,
        ClaimsPrincipal principal,
        IHRDashboardService hrService)
    {
        var hrId = GetHRId(principal);
        var dto = await hrService.SendInvitationAsync(hrId, request);
        return Results.Ok(dto);
    }

    public static async Task<IResult> GetInvitationsAsync(
        ClaimsPrincipal principal,
        IHRDashboardService hrService)
    {
        var hrId = GetHRId(principal);
        var list = await hrService.GetInvitationsAsync(hrId);
        return Results.Ok(list);
    }

    // ── Helper ────────────────────────────────────────────────────────────────

    private static int GetHRId(ClaimsPrincipal principal)
    {
        var raw = principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User ID not found in token.");

        return int.TryParse(raw, out var id)
            ? id
            : throw new UnauthorizedAccessException("Invalid user ID in token.");
    }
}