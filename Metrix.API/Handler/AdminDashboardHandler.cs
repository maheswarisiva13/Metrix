
using Metrix.Application.DTOs.Admin;
using Metrix.Application.Interfaces.Services;

namespace Metrix.API.Handlers;

public static class AdminDashboardHandler
{
    // GET /api/admin/dashboard
    public static async Task<IResult> GetDashboardAsync(IAdminDashboardService service)
    {
        var dashboard = await service.GetDashboardAsync();
        return Results.Ok(dashboard);
    }

    // GET /api/admin/security-users
    public static async Task<IResult> GetSecurityUsersAsync(IAdminDashboardService service)
    {
        var users = await service.GetSecurityUsersAsync();
        return Results.Ok(users);
    }

    // POST /api/admin/security-users
    public static async Task<IResult> CreateSecurityUserAsync(
        CreateSecurityUserRequest
         request,
        IAdminDashboardService service)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return Results.BadRequest(new
            {
                message = "Name, Email and Password are required."
            });
        }

        var createdUser = await service.CreateSecurityUserAsync(request);
        return Results.Ok(createdUser);
    }

    // POST /api/admin/security-users/{id}/deactivate
    public static async Task<IResult> DeactivateSecurityUserAsync(
        int id,
        IAdminDashboardService service)
    {
        await service.DeactivateSecurityUserAsync(id);

        return Results.Ok(new
        {
            message = "Security user deactivated successfully."
        });
    }

    // GET /api/admin/visitors
    public static async Task<IResult> GetAllVisitorsAsync(IAdminDashboardService service)
    {
        var visitors = await service.GetAllVisitorsAsync();
        return Results.Ok(visitors);
    }
}