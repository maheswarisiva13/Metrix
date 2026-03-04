
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Metrix.Application.Interfaces.Services;

namespace Metrix.Infrastructure.Services;

public class SmtpEmailService : IEmailService
{
    private readonly SmtpSetting _smtp;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration config, ILogger<SmtpEmailService> log)
    {
        _smtp = config.GetSection("Smtp").Get<SmtpSetting>()
                ?? throw new InvalidOperationException("Missing 'Smtp' section in appsettings.json");
        _logger = log;
    }

    // ── 1. Invitation → visitor gets registration link ────────────────────────
    public Task SendInvitationEmailAsync(
        string toEmail, string visitorName, string hrName,
        string purpose, DateTime visitDate, string registrationLink)
    {
        return SendAsync(toEmail, visitorName,
            subject: $"You're invited to visit — {visitDate:MMMM dd, yyyy}",
            html: InvitationHtml(visitorName, hrName, purpose, visitDate, registrationLink));
    }

    // ── 2. HR notification → visitor completed registration ───────────────────
    public Task SendVisitorRegisteredNotificationAsync(
        string hrEmail, string hrName,
        string visitorName, string purpose,
        DateTime visitDate, string dashboardUrl)
    {
        return SendAsync(hrEmail, hrName,
            subject: $"🔔 {visitorName} completed registration — Review now",
            html: HRNotificationHtml(hrName, visitorName, purpose, visitDate, dashboardUrl));
    }

    // ── 3. Approval → visitor gets Registration ID ────────────────────────────
    public Task SendApprovalEmailAsync(
        string toEmail, string visitorName,
        string registrationId, DateTime visitDate)
    {
        return SendAsync(toEmail, visitorName,
            subject: $"✅ Visit Approved — Your ID: {registrationId}",
            html: ApprovalHtml(visitorName, registrationId, visitDate));
    }

    // ── 4. Rejection → polite notice to visitor ───────────────────────────────
    public Task SendRejectionEmailAsync(
        string toEmail, string visitorName, string purpose)
    {
        return SendAsync(toEmail, visitorName,
            subject: "Update on your visit request — Metrix VMS",
            html: RejectionHtml(visitorName, purpose));
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  CORE SMTP SENDER
    // ─────────────────────────────────────────────────────────────────────────
    public async Task SendAsync(string toEmail, string toName, string subject, string html)
    {
        using var client = new SmtpClient(_smtp.Host, _smtp.Port)
        {
            Credentials = new NetworkCredential(_smtp.Username, _smtp.Password),
            EnableSsl = _smtp.EnableSsl,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            Timeout = 10_000,
        };

        using var msg = new MailMessage
        {
            From = new MailAddress(_smtp.FromEmail, _smtp.FromName),
            Subject = subject,
            Body = html,
            IsBodyHtml = true,
        };
        msg.To.Add(new MailAddress(toEmail, toName));

        try
        {
            await client.SendMailAsync(msg);
            _logger.LogInformation("[Email] ✓ '{Subject}' → {To}", subject, toEmail);
        }
        catch (SmtpException ex)
        {
            _logger.LogError(ex, "[Email] ✗ Failed to send '{Subject}' → {To}", subject, toEmail);
            throw new InvalidOperationException($"Failed to send email to {toEmail}.", ex);
        }
    }

    // ─────────────────────────────────────────────────────────────────────────
    //  HTML TEMPLATES
    // ─────────────────────────────────────────────────────────────────────────

    private static string Layout(string headerBg, string headerContent, string body) => $"""
        <!DOCTYPE html>
        <html><head><meta charset="UTF-8">
        <meta name="viewport" content="width=device-width,initial-scale=1"></head>
        <body style="margin:0;padding:0;background:#f0f4f8;font-family:'Segoe UI',Arial,sans-serif;">
        <table width="100%" cellpadding="0" cellspacing="0" style="padding:40px 16px;">
        <tr><td align="center">
          <table width="560" cellpadding="0" cellspacing="0"
                 style="background:#fff;border-radius:18px;overflow:hidden;
                        box-shadow:0 4px 28px rgba(0,0,0,.10);">

            <!-- Header -->
            <tr><td style="background:{headerBg};padding:28px 36px;text-align:center;">
              {headerContent}
            </td></tr>

            <!-- Body -->
            <tr><td style="padding:34px 36px 28px;">
              {body}
            </td></tr>

            <!-- Footer -->
            <tr><td style="background:#f8fafc;border-top:1px solid #e8edf3;
                           padding:14px 36px;text-align:center;">
              <p style="margin:0;color:#b0bac9;font-size:11px;">
                © {DateTime.UtcNow.Year} Metrix Visitor Management System
              </p>
            </td></tr>

          </table>
        </td></tr></table>
        </body></html>
        """;

    // ── Email 1: Invitation ───────────────────────────────────────────────────
    private static string InvitationHtml(
        string visitorName, string hrName,
        string purpose, DateTime visitDate, string link) =>
        Layout(
            headerBg: "#4ecdc4",
            headerContent: """
                <p style="margin:0;font-size:28px;font-weight:900;color:#fff;letter-spacing:-0.5px;">
                  Metrix<span style="opacity:.55">.</span>
                </p>
                <p style="margin:4px 0 0;color:rgba(255,255,255,.75);font-size:13px;">
                  Visitor Management System
                </p>
                """,
            body: $"""
                <h2 style="margin:0 0 6px;font-size:22px;font-weight:800;color:#1a2130;">
                  You're invited! 👋
                </h2>
                <p style="margin:0 0 24px;color:#4a5568;font-size:15px;line-height:1.75;">
                  Hi <strong>{visitorName}</strong>,<br>
                  <strong>{hrName}</strong> has invited you to visit our office.
                  Please complete your registration using the button below.
                </p>

                <table width="100%" cellpadding="0" cellspacing="0"
                       style="background:#e8faf9;border:1.5px solid #4ecdc4;
                              border-radius:12px;margin-bottom:28px;">
                <tr><td style="padding:18px 22px;">
                  <p style="margin:0 0 12px;font-size:10px;font-weight:700;
                             letter-spacing:1px;text-transform:uppercase;color:#2baaa0;">
                    Visit Details
                  </p>
                  <table cellpadding="5">
                    <tr>
                      <td style="color:#6b7a94;font-size:13px;padding-right:20px;">Purpose</td>
                      <td style="color:#1a2130;font-size:13px;font-weight:700;">{purpose}</td>
                    </tr>
                    <tr>
                      <td style="color:#6b7a94;font-size:13px;">Date</td>
                      <td style="color:#1a2130;font-size:13px;font-weight:700;">
                       {visitDate.ToLocalTime():dddd, MMMM dd, yyyy}
                      </td>
                    </tr>
                    <tr>
                      <td style="color:#6b7a94;font-size:13px;">Invited By</td>
                      <td style="color:#1a2130;font-size:13px;font-weight:700;">{hrName}</td>
                    </tr>
                  </table>
                </td></tr></table>

                <table width="100%" cellpadding="0" cellspacing="0">
                <tr><td align="center" style="padding-bottom:20px;">
                  <a href="{link}"
                     style="display:inline-block;padding:15px 52px;
                            background:#4ecdc4;color:#fff;font-size:15px;font-weight:800;
                            text-decoration:none;border-radius:50px;
                            box-shadow:0 4px 16px rgba(78,205,196,.4);">
                    Complete Registration →
                  </a>
                </td></tr></table>

                <p style="margin:0 0 18px;font-size:12px;color:#9aa5b4;text-align:center;">
                  Or copy this link:<br>
                  <a href="{link}" style="color:#4ecdc4;word-break:break-all;">{link}</a>
                </p>

                <div style="background:#fffbeb;border:1px solid #f59e0b;border-radius:10px;
                            padding:12px 16px;font-size:12px;color:#92400e;">
                  ⚠️ This link is personal and expires in <strong>7 days</strong>.
                  Do not share it with others.
                </div>
                """);

    // ── Email 2: HR Notification ──────────────────────────────────────────────
    private static string HRNotificationHtml(
        string hrName, string visitorName,
        string purpose, DateTime visitDate, string pendingUrl) =>
        Layout(
            headerBg: "#1a2130",
            headerContent: """
                <p style="margin:0;font-size:26px;font-weight:900;color:#4ecdc4;letter-spacing:-0.5px;">
                  Metrix<span style="color:#fff;opacity:.4">.</span>
                </p>
                <p style="margin:4px 0 0;color:rgba(255,255,255,.5);font-size:13px;">
                  Visitor Management System
                </p>
                """,
            body: $"""
                <div style="background:#fffbeb;border:1.5px solid #f59e0b;border-radius:12px;
                            padding:14px 18px;margin-bottom:24px;">
                  <p style="margin:0;font-size:14px;font-weight:800;color:#1a2130;">
                    🔔 Action Required
                  </p>
                  <p style="margin:4px 0 0;font-size:12px;color:#92400e;">
                    A visitor has completed registration and is awaiting your review.
                  </p>
                </div>

                <p style="margin:0 0 22px;color:#4a5568;font-size:15px;line-height:1.75;">
                  Hi <strong>{hrName}</strong>,<br>
                  <strong>{visitorName}</strong> has filled out their registration form.
                  Please log in to approve or reject their visit request.
                </p>

                <table width="100%" cellpadding="0" cellspacing="0"
                       style="background:#f8fafc;border:1px solid #e8edf3;
                              border-radius:12px;margin-bottom:26px;">
                <tr><td style="padding:18px 22px;">
                  <table cellpadding="7" width="100%">
                    <tr>
                      <td style="color:#6b7a94;font-size:13px;width:38%;">Visitor</td>
                      <td style="color:#1a2130;font-size:13px;font-weight:700;">{visitorName}</td>
                    </tr>
                    <tr>
                      <td style="color:#6b7a94;font-size:13px;">Purpose</td>
                      <td style="color:#1a2130;font-size:13px;font-weight:700;">{purpose}</td>
                    </tr>
                    <tr>
                      <td style="color:#6b7a94;font-size:13px;">Visit Date</td>
                      <td style="color:#1a2130;font-size:13px;font-weight:700;">
                        {visitDate.ToLocalTime():dddd, MMMM dd, yyyy}
                      </td>
                    </tr>
                    <tr>
                      <td style="color:#6b7a94;font-size:13px;">Submitted</td>
                      <td style="color:#1a2130;font-size:13px;font-weight:700;">
                        {DateTime.UtcNow:dd MMM yyyy, HH:mm} UTC
                      </td>
                    </tr>
                  </table>
                </td></tr></table>

                <table width="100%" cellpadding="0" cellspacing="0">
                <tr><td align="center">
                  <a href="{pendingUrl}"
                     style="display:inline-block;padding:14px 48px;
                            background:#4ecdc4;color:#fff;font-size:15px;font-weight:800;
                            text-decoration:none;border-radius:50px;">
                    Review Visitor →
                  </a>
                </td></tr></table>
                """);

    // ── Email 3: Approval with Registration ID ────────────────────────────────
    private static string ApprovalHtml(
        string visitorName, string registrationId, DateTime visitDate) =>
        Layout(
            headerBg: "#22c55e",
            headerContent: $"""
                <p style="margin:0 0 10px;font-size:44px;">✅</p>
                <p style="margin:0;font-size:22px;font-weight:800;color:#fff;">
                  Your Visit is Approved!
                </p>
                """,
            body: $"""
                <p style="margin:0 0 22px;color:#4a5568;font-size:15px;line-height:1.75;">
                  Hi <strong>{visitorName}</strong>,<br>
                  Your visit has been <strong>approved</strong>.
                  Show the Registration ID below to the security team when you arrive.
                </p>

                <table width="100%" cellpadding="0" cellspacing="0"
                       style="background:#f0fdf4;border:2px solid #22c55e;
                              border-radius:14px;margin-bottom:24px;">
                <tr><td style="padding:26px;text-align:center;">
                  <p style="margin:0 0 8px;font-size:10px;font-weight:700;
                             letter-spacing:1.2px;text-transform:uppercase;color:#16a34a;">
                    Your Registration ID
                  </p>
                  <p style="margin:0;font-size:36px;font-weight:900;color:#15803d;
                             letter-spacing:4px;font-family:'Courier New',monospace;">
                    {registrationId}
                  </p>
                  <p style="margin:10px 0 0;font-size:12px;color:#6b7a94;">
                    Show this to security staff at the building entrance
                  </p>
                </td></tr></table>

                <div style="background:#f8fafc;border:1px solid #e8edf3;border-radius:10px;
                            padding:14px 18px;font-size:14px;color:#4a5568;">
                  📅 <strong>Visit Date:</strong> {visitDate.ToLocalTime():dddd, MMMM dd, yyyy}
                </div>
                """);

    // ── Email 4: Rejection ────────────────────────────────────────────────────
    private static string RejectionHtml(string visitorName, string purpose) =>
        Layout(
            headerBg: "#64748b",
            headerContent: """
                <p style="margin:0;font-size:22px;font-weight:800;color:#fff;">
                  Visit Request Update
                </p>
                """,
            body: $"""
                <p style="margin:0 0 18px;color:#4a5568;font-size:15px;line-height:1.75;">
                  Hi <strong>{visitorName}</strong>,<br>
                  We regret to inform you that your visit request for
                  <strong>{purpose}</strong> could not be approved at this time.
                </p>
                <p style="margin:0 0 18px;color:#4a5568;font-size:15px;line-height:1.75;">
                  Please contact the HR team who sent your invitation if you need
                  further information or would like to reschedule.
                </p>
                <p style="margin:0;color:#9aa5b4;font-size:13px;">
                  We apologise for any inconvenience caused.
                </p>
                """);
}

// ─────────────────────────────────────────────────────────────────────────────
//  Config binding for appsettings.json "Smtp" section
// ─────────────────────────────────────────────────────────────────────────────
public class SmtpSetting
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool EnableSsl { get; set; } = true;
    public string FromEmail { get; set; } = string.Empty;
    public string FromName { get; set; } = "Metrix";
}