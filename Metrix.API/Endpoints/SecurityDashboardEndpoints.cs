using Metrix.API.Handler;

namespace Metrix.API.Endpoints
{
    public static class SecurityDashboardEndpoints
    {
        public static void MapSecurityDashboardEndpoints(this WebApplication app)
        {
            var sec = app.MapGroup("/api/security")
                         .WithTags("Security")
                         .RequireAuthorization("SecurityPolicy");

            sec.MapGet("/dashboard", SecurityDashboardHandler.GetDashboardAsync)
               .WithName("SecurityDashboard");

            sec.MapGet("/visitors/checked-in", SecurityDashboardHandler.GetCheckedInAsync)
               .WithName("GetCheckedInVisitors");

            sec.MapGet("/visitors/today", SecurityDashboardHandler.GetTodayVisitorsAsync)
               .WithName("GetTodayVisitors");


            sec.MapGet("/visitors/all", SecurityDashboardHandler.GetAllVisitorsAsync)
               .WithName("GetAllVisitorsSecurity");


            sec.MapGet("/visitor/lookup", SecurityDashboardHandler.LookupVisitorAsync)
               .WithName("LookupVisitor");

            sec.MapGet("/logs/today", SecurityDashboardHandler.GetTodayLogsAsync)
               .WithName("GetTodayLogs");

            sec.MapPost("/visitor/{id:int}/check-in", SecurityDashboardHandler.CheckInAsync)
               .WithName("CheckInVisitor");

            sec.MapPost("/visitor/{id:int}/check-out", SecurityDashboardHandler.CheckOutAsync)
               .WithName("CheckOutVisitor");

            // GET /api/security/visitors/history
            sec.MapGet("/visitors/history", SecurityDashboardHandler.GetVisitorHistoryAsync)
               .WithName("GetVisitorHistory");


        }
    }
}