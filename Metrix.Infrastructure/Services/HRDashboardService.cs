using Metrix.Application.DTOs.HR;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;
using Metrix.Domain.Entities;
using Metrix.Domain.Enums;
using Microsoft.Extensions.Configuration;
using System.Xml.Linq;

namespace Metrix.Infrastructure.Services;

public class HRDashboardService : IHRDashboardService
{
    private readonly IHRDashboardRepository _repo;   // Dashboard data
    private readonly IHRRepository _hrRepo;          // HR user data
    private readonly IEmailService _email;
    private readonly string _frontendBase;

    public HRDashboardService(
        IHRDashboardRepository repo,
        IHRRepository hrRepo,
        IEmailService email,
        IConfiguration config)
    {
        _repo = repo;
        _hrRepo = hrRepo;
        _email = email;
        _frontendBase = config["Frontend:BaseUrl"] ?? "http://localhost:5173";
    }

    // ───────────────── DASHBOARD ─────────────────

    /*public async Task<HRDashboardStatsDto> GetDashboardStatsAsync(int hrId)
    {
        var t1 = _repo.CountInvitationsByHRAsync(hrId);
        var t2 = _repo.CountPendingByHRAsync(hrId);
        var t3 = _repo.CountApprovedTodayByHRAsync(hrId);
        var t4 = _repo.CountTotalVisitorsByHRAsync(hrId);
        var t5 = _repo.CountTodayVisitorsByHRAsync(hrId);

        await Task.WhenAll(t1, t2, t3, t4, t5);

        return new HRDashboardStatsDto
        {
            TotalInvitations = t1.Result,
            PendingApprovals = t2.Result,
            ApprovedToday = t3.Result,
            TotalVisitors = t4.Result,
            TodayVisitors = t5.Result
        };
    }*/
    public async Task<HRDashboardStatsDto> GetDashboardStatsAsync(int hrId)
    {
        var totalInvitations = await _repo.CountInvitationsByHRAsync(hrId);
        var pendingApprovals = await _repo.CountPendingByHRAsync(hrId);
        var approvedToday = await _repo.CountApprovedTodayByHRAsync(hrId);
        var totalVisitors = await _repo.CountTotalVisitorsByHRAsync(hrId);
        var todayVisitors = await _repo.CountTodayVisitorsByHRAsync(hrId);

        return new HRDashboardStatsDto
        {
            TotalInvitations = totalInvitations,
            PendingApprovals = pendingApprovals,
            ApprovedToday = approvedToday,
            TotalVisitors = totalVisitors,
            TodayVisitors = todayVisitors
        };
    }

    // ───────────────── VISITORS ─────────────────

    public async Task<List<VisitorDto>> GetAllVisitorsAsync(int hrId)
        => (await _repo.GetAllVisitorsByHRAsync(hrId)).Select(ToDto).ToList();

    public async Task<List<VisitorDto>> GetPendingVisitorsAsync(int hrId)
        => (await _repo.GetPendingVisitorsByHRAsync(hrId)).Select(ToDto).ToList();

    public async Task<List<VisitorDto>> GetRecentVisitorsAsync(int hrId)
        => (await _repo.GetRecentVisitorsByHRAsync(hrId)).Select(ToDto).ToList();

    public async Task<ApproveVisitorDto> ApproveVisitorAsync(int visitorId, int hrId)
    {
        var visitor = await _repo.GetVisitorByIdAsync(visitorId)
            ?? throw new KeyNotFoundException($"Visitor with ID {visitorId} was not found.");

        if (visitor.Invitation?.CreatedByHRId != hrId)
            throw new UnauthorizedAccessException("You are not authorised to approve this visitor.");

        if (visitor.Status != VisitorStatus.Pending)
            throw new InvalidOperationException($"Visitor is already {visitor.Status}.");

        var year = DateTime.UtcNow.Year;
        var totalCount = await _repo.CountAllApprovedByHRAsync(hrId);
        var regId = $"VIS-{year}-{(totalCount + 1):D4}";

        visitor.Status = VisitorStatus.Approved;
        visitor.RegistrationId = regId;
        visitor.ApprovedByHRId = hrId;
        visitor.ApprovedAt = DateTime.UtcNow;

        if (visitor.Invitation is not null)
            visitor.Invitation.Status = InvitationStatus.Approved;

        await _repo.UpdateVisitorAsync(visitor);

        await _email.SendApprovalEmailAsync(
            visitor.Email,
            visitor.Name,
            regId,
            visitor.Invitation?.VisitDate ?? DateTime.UtcNow
        );

        return new ApproveVisitorDto
        {
            RegistrationId = regId,
            Message = $"Visitor approved successfully. Registration ID: {regId}"
        };
    }

    public async Task<RejectVisitorDto> RejectVisitorAsync(int visitorId, int hrId)
    {
        var visitor = await _repo.GetVisitorByIdAsync(visitorId)
            ?? throw new KeyNotFoundException($"Visitor with ID {visitorId} was not found.");

        if (visitor.Invitation?.CreatedByHRId != hrId)
            throw new UnauthorizedAccessException("You are not authorised to reject this visitor.");

        if (visitor.Status != VisitorStatus.Pending)
            throw new InvalidOperationException($"Visitor is already {visitor.Status}.");

        visitor.Status = VisitorStatus.Rejected;

        if (visitor.Invitation is not null)
            visitor.Invitation.Status = InvitationStatus.Rejected;

        await _repo.UpdateVisitorAsync(visitor);

        await _email.SendRejectionEmailAsync(
            visitor.Email,
            visitor.Name,
            visitor.Invitation?.Purpose ?? string.Empty
        );

        return new RejectVisitorDto
        {
            Success = true,
            Message = "Visitor has been rejected."
        };
    }

    // ───────────────── INVITATIONS ─────────────────

    public async Task<SendInvitationDto> SendInvitationAsync(int hrId, SendInvitationRequest request)
    {
        var token = Guid.NewGuid().ToString("N")[..16].ToUpper();

        var invitation = new Invitation
        {
            VisitorName = request.VisitorName.Trim(),
            VisitorEmail = request.VisitorEmail.Trim().ToLower(),
            Purpose = request.Purpose.Trim(),
            VisitDate = DateTime.SpecifyKind(request.VisitDate, DateTimeKind.Utc),
            Token = token,
            Status = InvitationStatus.Pending,
            CreatedByHRId = hrId
        };

        await _repo.CreateInvitationAsync(invitation);

        // 🔹 FIXED: Now using _hrRepo (correct repository)
      // var hr = await _hrRepo.GetByIdAsync(hrId);

        var link = $"{_frontendBase}/register?token={token}";

       await _email.SendInvitationEmailAsync(
            request.VisitorEmail,
            request.VisitorName,
             "Greetings From HR Team",
            request.Purpose,
            request.VisitDate,
            link
        );
 

        return new SendInvitationDto
        {
            Token = token,
            InviteLink = link
        };
    }

    public async Task<List<InvitationDto>> GetInvitationsAsync(int hrId)
    {
        var invitations = await _repo.GetInvitationsByHRAsync(hrId);

        return invitations.Select(i => new InvitationDto
        {
            Id = i.Id,
            VisitorName = i.VisitorName,
            VisitorEmail = i.VisitorEmail,
            Purpose = i.Purpose,
            VisitDate = i.VisitDate,
            Token = i.Token,
            Status = i.Status.ToString(),
            SentAt = DateTime.UtcNow,
            HRName = i.CreatedByHR?.Name ?? string.Empty
        }).ToList();
    }

    // ───────────────── MAPPER ─────────────────

    private static VisitorDto ToDto(Visitor v) => new()
    {
        Id = v.Id,
        Name = v.Name,
        Email = v.Email,
        Phone = v.Phone,
        Purpose = v.Invitation?.Purpose ?? string.Empty,
        VisitDate = v.Invitation?.VisitDate ?? default,
        IdProofType = v.IDProofType.ToString(),
        IdProofNumber = v.IDProofNumber,
        PhotoPath = v.PhotoPath,
        RegistrationId = v.RegistrationId,
        Status = v.Status.ToString(),
        SubmittedAt = v.SubmittedAt,
        ApprovedAt = v.ApprovedAt,
        ApprovedByHR = v.ApprovedByHR?.Name,
        HRName = v.Invitation?.CreatedByHR?.Name ?? string.Empty,
       // InvitedByEmail = v.Invitation?.CreatedByHR?.Email ?? string.Empty
    };
}