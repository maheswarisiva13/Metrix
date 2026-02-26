using Metrix.Application.DTOs.Auth;
using Metrix.Application.Interfaces;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;

public class AuthService : IAuthService
{
    private readonly IHRRepository _hrRepo;
    private readonly ISecurityUserRepository _securityRepo;
    private readonly IAdminRepository _adminRepo;
    private readonly IJwtGenerator _jwtGenerator;

    public AuthService(
        IHRRepository hrRepo,
        ISecurityUserRepository securityRepo,
        IAdminRepository adminRepo,
        IJwtGenerator jwtGenerator)
    {
        _hrRepo = hrRepo;
        _securityRepo = securityRepo;
        _adminRepo = adminRepo;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        // ================= ADMIN =================
        var admin = await _adminRepo.GetByEmailAsync(request.Email);
        if (admin != null && BCrypt.Net.BCrypt.Verify(request.Password, admin.PasswordHash))
        {
            var token = _jwtGenerator.GenerateToken(admin.Id.ToString(), admin.Email, "Admin");

            return new LoginResponseDto
            {
                Token = token,
                Role = "Admin",
                Name = admin.Name
            };
        }

        // ================= HR =================
        var hr = await _hrRepo.GetByEmailAsync(request.Email);
        if (hr != null && BCrypt.Net.BCrypt.Verify(request.Password, hr.PasswordHash))
        {
            var token = _jwtGenerator.GenerateToken(hr.Id.ToString(), hr.Email, "HR");

            return new LoginResponseDto
            {
                Token = token,
                Role = "HR",
                Name = hr.Name
            };
        }

        // ================= SECURITY =================
        var security = await _securityRepo.GetByEmailAsync(request.Email);
        if (security != null && BCrypt.Net.BCrypt.Verify(request.Password, security.PasswordHash))
        {
            var token = _jwtGenerator.GenerateToken(security.Id.ToString(), security.Email, "Security");

            return new LoginResponseDto
            {
                Token = token,
                Role = "Security",
                Name = security.Name
            };
        }

        return null;
    }
}