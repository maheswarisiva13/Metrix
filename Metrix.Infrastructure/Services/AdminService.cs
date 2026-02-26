using Metrix.Application.DTOs.Admin;
using Metrix.Application.DTOs.Security;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;
using Metrix.Domain.Entities;
using Metrix.Domain.Enums;

public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;

    public AdminService(IAdminRepository adminRepository)
    {
        _adminRepository = adminRepository;
    }

    public async Task<string> CreateAdminAsync(RegisterAdminRequestDto dto)
    {
        var adminExists = await _adminRepository.AdminExistsAsync();

        if (adminExists)
            throw new Exception("Admin already exists.");

        if (dto.Password != dto.ConfirmPassword)
            throw new Exception("Passwords do not match.");

        var admin = new User
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };

        await _adminRepository.AddAdminAsync(admin);
        await _adminRepository.SaveChangesAsync();

        return "Admin created successfully.";
    }

    public async Task<string> CreateSecurityAsync(RegisterSecurityRequestDto dto)
    {
        if (dto.Password != dto.ConfirmPassword)
            throw new Exception("Passwords do not match.");

        var emailExists = await _adminRepository.SecurityEmailExistsAsync(dto.Email);

        if (emailExists)
            throw new Exception("Security email already exists.");

        var security = new SecurityUser
        {
            Name = dto.Name,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _adminRepository.AddSecurityAsync(security);
        await _adminRepository.SaveChangesAsync();

        return "Security user created successfully.";
    }
}