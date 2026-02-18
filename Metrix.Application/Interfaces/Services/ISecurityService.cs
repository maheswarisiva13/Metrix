using Metrix.Application.DTOs.Security;

public interface ISecurityService
{
    Task<Guid> RegisterSecurityAsync(RegisterSecurityRequestDto request);
}
