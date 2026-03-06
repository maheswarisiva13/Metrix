using Metrix.Application.Interfaces.Services;
using System.Security.Claims;

namespace Metrix.API.Handler
{
    public static class SecurityDashboardHandler
    {
        // GET /api/security/dashboard
        public static async Task<IResult> GetDashboardAsync(
            ClaimsPrincipal principal,
            ISecurityDashboardService service)
        {
            var id = ExtractId(principal);
            var dto = await service.GetDashboardAsync(id);
            return Results.Ok(dto);
        }

        // GET /api/security/visitors/checked-in
        public static async Task<IResult> GetCheckedInAsync(ISecurityDashboardService service)
        {
            var list = await service.GetCheckedInVisitorsAsync();
            return Results.Ok(list);
        }

        // GET /api/security/visitors/today
        public static async Task<IResult> GetTodayVisitorsAsync(ISecurityDashboardService service)
        {
            var list = await service.GetTodayVisitorsAsync();
            return Results.Ok(list);
        }

        // GET /api/security/visitors/all
        public static async Task<IResult> GetAllVisitorsAsync(ISecurityDashboardService service)
        {
            var list = await service.GetAllVisitorsAsync();
            return Results.Ok(list);
        }

        // GET /api/security/visitor/lookup?registrationId=VIS-2026-0001
        public static async Task<IResult> LookupVisitorAsync(
            string registrationId,
            ISecurityDashboardService service)
        {
            if (string.IsNullOrWhiteSpace(registrationId))
                return Results.BadRequest(new { message = "registrationId is required." });

            var visitor = await service.LookupVisitorAsync(registrationId);
            return visitor is null
                ? Results.NotFound(new { message = $"No visitor found with Registration ID '{registrationId}'." })
                : Results.Ok(visitor);
        }

        // GET /api/security/logs/today
        public static async Task<IResult> GetTodayLogsAsync(ISecurityDashboardService service)
        {
            var logs = await service.GetTodayLogsAsync();
            return Results.Ok(logs);
        }

        // POST /api/security/visitor/{id}/check-in
        public static async Task<IResult> CheckInAsync(
            int id,
            ClaimsPrincipal principal,
            ISecurityDashboardService service)
        {
            var secId = ExtractId(principal);
            var result = await service.CheckInAsync(id, secId);
            return Results.Ok(result);
        }

        // POST /api/security/visitor/{id}/check-out
        public static async Task<IResult> CheckOutAsync(
            int id,
            ClaimsPrincipal principal,
            ISecurityDashboardService service)
        {
            var secId = ExtractId(principal);
            var result = await service.CheckOutAsync(id, secId);
            return Results.Ok(result);
        }
        // GET /api/security/visitors/history
        public static async Task<IResult> GetVisitorHistoryAsync(
            ISecurityDashboardService service)
        {
            var visitors = await service.GetVisitorHistoryAsync();
            return Results.Ok(visitors);
        }

        // ── helper ────────────────────────────────────────────────────────────────

        private static int ExtractId(ClaimsPrincipal principal)
        {
            var raw = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User ID claim not found.");
            return int.TryParse(raw, out var id) ? id
                : throw new UnauthorizedAccessException("Invalid user ID in token.");
        }
    }
}
