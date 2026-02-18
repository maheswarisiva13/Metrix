using Metrix.Application.DTOs.HR;

public interface IHrService
{
    Task<string> RegisterHrAsync(RegisterHrRequestDto request);
}
