using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.Interfaces.Services
{


    public interface IEmailService
    {
        Task SendAsync(string toEmail, string toName, string subject, string html);

        // ── existing (keep) ──────────────────────────────────────────────────────
        Task SendInvitationEmailAsync(
        string toEmail, string visitorName, string hrName,
        string purpose, DateTime visitDate, string inviteLink);

        /// <summary>HR receives "review now" notification after visitor self-registers.</summary>
       /* Task SendVisitorRegisteredNotificationAsync(
            string hrEmail, string hrName,
            string visitorName, string purpose,
            DateTime visitDate, string pendingUrl);*/


        Task SendApprovalEmailAsync(
            string toEmail, string visitorName,
            string registrationId, DateTime visitDate);

        Task SendRejectionEmailAsync(
            string toEmail, string visitorName, string purpose);

        // ── NEW ───────────────────────────────────────────────────────────────────

        /// <summary>
        /// Sent to HR when a visitor completes self-registration.
        /// HR sees a "Review now" button that links to /hr/pending.
        /// </summary>
        Task SendVisitorRegisteredNotificationAsync(
            string hrEmail,
            string hrName,
            string visitorName,
            string purpose,
            DateTime visitDate,
            string dashboardUrl);

    }
}