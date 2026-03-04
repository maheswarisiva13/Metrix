using Metrix.Domain.Enums;

namespace Metrix.Domain.Entities;

public class Invitation
{
    public int Id { get; set; }

    public string VisitorName { get; set; } = string.Empty;

    public string VisitorEmail { get; set; } = string.Empty;

    public string Purpose { get; set; } = string.Empty;

    public DateTime VisitDate { get; set; }

    public string Token { get; set; } = string.Empty;

    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;
   
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int CreatedByHRId { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public HRUser? CreatedByHR { get; set; }
}
