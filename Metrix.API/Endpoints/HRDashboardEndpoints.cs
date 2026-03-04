using Metrix.API.Handlers;
using Metrix.API.Constants;

namespace Metrix.API.Endpoints;

public static class HRDashboardEndpoints
{
    public static void MapHRDashboardEndpoints(this WebApplication app)
    {
        var hr = app.MapGroup(ApiRoutes.Hr.Root)
                    .WithTags("HR Dashboard")
                    .RequireAuthorization("HRPolicy");

        hr.MapGet("/dashboard", HRDashboardHandlers.GetDashboardStatsAsync)
          .WithName("HRDashboard");

        hr.MapGet("/visitors/pending", HRDashboardHandlers.GetPendingVisitorsAsync)
          .WithName("GetPendingVisitors");

        hr.MapGet("/visitors/recent", HRDashboardHandlers.GetRecentVisitorsAsync)
          .WithName("GetRecentVisitors");

        hr.MapGet("/visitors", HRDashboardHandlers.GetAllVisitorsAsync)
          .WithName("GetAllVisitors");

        hr.MapPost("/visitors/{id:int}/approve", HRDashboardHandlers.ApproveVisitorAsync)
          .WithName("ApproveVisitor");

        hr.MapPost("/visitors/{id:int}/reject", HRDashboardHandlers.RejectVisitorAsync)
          .WithName("RejectVisitor");


     /* var inv = app.MapGroup(ApiRoutes.Invitation.Root)
                     .WithTags("Invitations")
                     .RequireAuthorization("HRPolicy");

        inv.MapPost("/send", HRDashboardHandlers.SendInvitationAsync)
           .WithName("SendInvitation");

        inv.MapGet("/", HRDashboardHandlers.GetInvitationsAsync)
           .WithName("GetInvitations");*/
    }
}