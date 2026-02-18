using Metrix.Domain.Enums;

namespace Metrix.Domain.Entities;

public class Visitor
{
    public Guid Id { get; set; }

    public Guid InvitationId { get; set; }
    public Invitation? Invitation { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string Company { get; set; } = string.Empty;

    public IDProofType IDProofType { get; set; }

    public string IDProofNumber { get; set; } = string.Empty;

    public string PhotoPath { get; set; } = string.Empty;

    public string? RegistrationId { get; set; }

    public VisitorStatus Status { get; set; } = VisitorStatus.Pending;

    public Guid? ApprovedByHRId { get; set; }

    public DateTime? ApprovedAt { get; set; }
}
