using Metrix.Application.DTOs.Admin;



namespace Metrix.Application.Interfaces.Services;

public interface IAdminDashboardService
{
    Task<AdminDashboardDto> GetDashboardAsync();
    Task<List<SecurityUserDto>> GetSecurityUsersAsync();
    Task<SecurityUserDto> CreateSecurityUserAsync(CreateSecurityUserRequest request);
    Task DeactivateSecurityUserAsync(int id);
    Task<List<AdminVisitorDto>> GetAllVisitorsAsync();
}