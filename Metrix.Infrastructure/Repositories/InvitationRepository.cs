using Metrix.Application.Interfaces.Repositories;
using Metrix.Domain.Entities;
using Metrix.Infrastructure.Persistence;

namespace Metrix.Infrastructure.Repositories;

public class InvitationRepository : IInvitationRepository
{
    private readonly AppDbContext _context;

    public InvitationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Invitation invitation)
    {
        await _context.Invitations.AddAsync(invitation);
    }

   


    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
