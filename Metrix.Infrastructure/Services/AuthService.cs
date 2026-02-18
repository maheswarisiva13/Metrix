using Metrix.Application.DTOs.Auth;
using Metrix.Application.Interfaces;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;

public class AuthService : IAuthService
{
    private readonly IHRRepository _hrRepo;
    private readonly ISecurityUserRepository _securityRepo;
    private readonly IJwtGenerator _jwtGenerator; // ✅ use interface

    public AuthService(
        IHRRepository hrRepo,
        ISecurityUserRepository securityRepo,
        IJwtGenerator jwtGenerator) // ✅ inject interface
    {
        _hrRepo = hrRepo;
        _securityRepo = securityRepo;
        _jwtGenerator = jwtGenerator;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        // Check HR
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

        // Check Security
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
