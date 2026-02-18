using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.Interfaces.Services
{
    public interface IInvitationService
    {
        Task SendInvitationAsync(
            string visitorName,
            string visitorEmail,
            string purpose,
            DateTime visitDate,
            Guid createdByHrId);
    }
}
