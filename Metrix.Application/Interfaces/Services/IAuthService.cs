using Metrix.Application.Common;
using Metrix.Application.DTOs.Auth;

namespace Metrix.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
}
