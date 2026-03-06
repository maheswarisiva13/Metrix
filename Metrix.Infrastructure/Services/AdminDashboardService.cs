using Metrix.Application.DTOs.Admin;

using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;
using Metrix.Domain.Entities;

namespace Metrix.Infrastructure.Services;

public class AdminDashboardService : IAdminDashboardService
{
    private readonly IAdminDashboardRepository _repository;

    public AdminDashboardService(IAdminDashboardRepository repository)
        => _repository = repository;

    // ── Dashboard stats ───────────────────────────────────────────────────────

    public async Task<AdminDashboardDto> GetDashboardAsync()
    {
        //var t1 = _repository.CountSecurityUsersAsync();
        var totalsecurity = await _repository.CountSecurityUsersAsync();
        var activeUsers = await _repository.CountActiveSecurityUsersAsync();
        var totalVisitors = await _repository.CountTotalVisitorsAsync();
        var checkincount = await _repository.CountCheckedInTodayAsync();
        var checkoutcount = await _repository.CountCheckedOutTodayAsync();

        return new AdminDashboardDto
        {
            TotalSecurityUsers=totalsecurity,
            ActiveSecurityUsers = activeUsers,
            TotalVisitors = totalVisitors,
            CheckedInCount = checkincount,
            CheckedOutCount = checkoutcount,
        };
    }

    // ── Security users ────────────────────────────────────────────────────────

    public async Task<List<SecurityUserDto>> GetSecurityUsersAsync()
        => (await _repository.GetAllSecurityUsersAsync())
           .Select(ToDto)
           .ToList();

    public async Task<SecurityUserDto> CreateSecurityUserAsync(CreateSecurityUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new InvalidOperationException("Name is required.");

        if (string.IsNullOrWhiteSpace(request.Email))
            throw new InvalidOperationException("Email is required.");

        if (request.Password.Length < 6)
            throw new InvalidOperationException("Password must be at least 6 characters.");

        var email = request.Email.Trim().ToLower();

        if (await _repository.SecurityEmailExistsAsync(email))
            throw new InvalidOperationException($"A security account with email '{email}' already exists.");

        var user = new SecurityUser
        {
            Name = request.Name.Trim(),
            Email = email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateSecurityUserAsync(user);

        return ToDto(created);
    }

    public async Task DeactivateSecurityUserAsync(int id)
        => await _repository.DeactivateSecurityUserAsync(id);

    // ── Visitors ──────────────────────────────────────────────────────────────

    public async Task<List<AdminVisitorDto>> GetAllVisitorsAsync()
        => (await _repository.GetAllVisitorsAsync())
           .Select(ToVisitorDto)
           .ToList();

    // ── Mappers ───────────────────────────────────────────────────────────────

    private static SecurityUserDto ToDto(SecurityUser u) => new()
    {
        Id = u.Id,
        Name = u.Name,
        Email = u.Email,
        IsActive = u.IsActive,
        CreatedAt = u.CreatedAt
    };

    private static AdminVisitorDto ToVisitorDto(Visitor v) => new()
    {
        Id = v.Id,
        Name = v.Name,
        Email = v.Email,
        Phone = v.Phone,
        Purpose = v.Invitation?.Purpose ?? string.Empty,
        VisitDate = v.Invitation?.VisitDate ?? default,
        IdProofType = v.IDProofType.ToString(),
        IdProofNumber = v.IDProofNumber,
        RegistrationId = v.RegistrationId,
        Status = v.Status.ToString(),
        HrName = v.Invitation?.CreatedByHR?.Name ?? string.Empty,
        SubmittedAt = v.SubmittedAt,
        ApprovedAt = v.ApprovedAt,
        ApprovedByHR = v.ApprovedByHR?.Name,
       
    };
}

