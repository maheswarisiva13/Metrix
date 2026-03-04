using Metrix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.Interfaces.Repositories
{
    public interface IInvitationRepository
    {
        Task AddAsync(Invitation invitation);
        Task SaveChangesAsync();

        
    }
}
