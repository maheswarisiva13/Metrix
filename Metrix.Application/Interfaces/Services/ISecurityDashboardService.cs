using Metrix.Application.DTOs.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.Interfaces.Services
{
    public interface ISecurityDashboardService
    {
        Task<SecurityDashboardDto> GetDashboardAsync(int securityId);
        Task<SecurityVisitorDto?> LookupVisitorAsync(string registrationId);
        Task<List<SecurityVisitorDto>> GetCheckedInVisitorsAsync();
        Task<List<SecurityVisitorDto>> GetTodayVisitorsAsync();
        Task<List<SecurityVisitorDto>> GetAllVisitorsAsync();
        Task<List<VisitLogDto>> GetTodayLogsAsync();
        Task<CheckInOutDto> CheckInAsync(int visitorId, int securityId);
        Task<CheckInOutDto> CheckOutAsync(int visitorId, int securityId);
        Task<List<SecurityVisitorDto>> GetVisitorHistoryAsync();
    }
}
