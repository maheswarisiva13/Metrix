using Metrix.Application.DTOs.Security;

public interface ISecurityService
{
    Task<string> RegisterSecurityAsync(RegisterSecurityRequestDto request);
}
