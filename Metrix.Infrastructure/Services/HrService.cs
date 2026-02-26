using Metrix.Application.DTOs;
using Metrix.Application.DTOs.HR;
using Metrix.Application.Interfaces;
using Metrix.Application.Interfaces.Repositories;
using Metrix.Domain.Entities;

public class HrService : IHrService
{
    private readonly IHRRepository _hrRepository;

    public HrService(IHRRepository hrRepository)
    {
        _hrRepository = hrRepository;
    }

    public async Task<string> RegisterHrAsync(RegisterHrRequestDto request)
    {
        // 1️⃣ Check if email already exists
        var existing = await _hrRepository.GetByEmailAsync(request.Email);

        if (existing != null)
            throw new Exception("Email already exists");

        // 2️⃣ Hash password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 3️⃣ Create entity
        var user = new HRUser
        {
           
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash
        };

        // 4️⃣ Save to DB
        await _hrRepository.AddAsync(user);

        return "HR user created successfully";
    }
}
