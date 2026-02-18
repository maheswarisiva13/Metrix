using Metrix.Application.DTOs.Security;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Application.Interfaces.Services;
using Metrix.Domain.Entities;

public class SecurityService : ISecurityService
{
    private readonly ISecurityUserRepository _repository;

    public SecurityService(ISecurityUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> RegisterSecurityAsync(RegisterSecurityRequestDto request)
    {
        var existing = await _repository.GetByEmailAsync(request.Email);

        if (existing != null)
            throw new ApplicationException("Email already exists");

        var user = new SecurityUser
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _repository.AddAsync(user);

        return user.Id;
    }
}
