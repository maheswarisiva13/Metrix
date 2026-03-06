using Metrix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.Interfaces.Repositories
{
    public interface ISecurityDashboardRepository
    {
        // ── Auth ──────────────────────────────────────────────────────────────────
        Task<SecurityUser?> GetByEmailAsync(string email);

        // ── Visitors ──────────────────────────────────────────────────────────────
        Task<Visitor?> GetVisitorByRegistrationIdAsync(string registrationId);
        Task<Visitor?> GetVisitorByIdAsync(int id);
        Task<List<Visitor>> GetCheckedInVisitorsAsync();
        Task<List<Visitor>> GetTodayVisitorsAsync();
        Task<List<Visitor>> GetAllVisitorsAsync();
        Task UpdateVisitorAsync(Visitor visitor);

        // ── Visit Logs ────────────────────────────────────────────────────────────
        Task<VisitLog?> GetOpenLogAsync(int visitorId);
        Task<VisitLog> CreateLogAsync(VisitLog log);
        Task UpdateLogAsync(VisitLog log);
        Task<List<VisitLog>> GetTodayLogsAsync();

        // ── Stats ─────────────────────────────────────────────────────────────────
        Task<int> CountTodayVisitorsAsync();
        Task<int> CountCheckedInAsync();
        Task<int> CountCheckedInTodayAsync();
        Task<int> CountCheckedOutTodayAsync();

        Task<List<Visitor>> GetVisitorHistoryAsync();

    }
}
