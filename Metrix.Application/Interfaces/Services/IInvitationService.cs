using Metrix.Application.DTOs.Invitation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.Interfaces.Services
{
    public interface IInvitationService
    {
        /// Sends an invitation to a visitor and returns the registration link.
        /// </summary>
        Task<InvitationResponseDto> SendInvitationAsync(
            string visitorName,
            string visitorEmail,
            string purpose,
            DateTime visitDate,
            int createdByHrId
        );
    }
}

