using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.DTOs.Invitation
{
    public class SendInvitationDto
    {
        public string VisitorName { get; set; } = string.Empty;
        public string VisitorEmail { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public int CreatedByHrId { get; set; }
    }
}
