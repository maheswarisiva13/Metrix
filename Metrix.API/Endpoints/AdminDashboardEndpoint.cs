using Metrix.API.Handlers;

namespace Metrix.API.Endpoints;

public static class AdminDashboardEndpoint
{
    public static void MapAdminDashboardEndpoints(this WebApplication app)
    {
        var admin = app.MapGroup("/api/admin")
                       .WithTags("Admin")
                       .RequireAuthorization("AdminPolicy");

        // GET /api/admin/dashboard
        admin.MapGet("/dashboard", AdminDashboardHandler.GetDashboardAsync)
             .WithName("AdminDashboard");


        // GET /api/admin/security-users
        admin.MapGet("/security-users", AdminDashboardHandler.GetSecurityUsersAsync)
             .WithName("GetSecurityUsers");


        // POST /api/admin/security-users
       admin.MapPost("/security-users", AdminDashboardHandler.CreateSecurityUserAsync)
            .WithName("CreateSecurityUser");


        // POST /api/admin/security-users/{id}/deactivate
        admin.MapPost("/security-users/{id:int}/deactivate", AdminDashboardHandler.DeactivateSecurityUserAsync)
             .WithName("DeactivateSecurityUser");


        // GET /api/admin/visitors
        admin.MapGet("/visitors", AdminDashboardHandler.GetAllVisitorsAsync)
             .WithName("AdminGetAllVisitors");
             
    }
}