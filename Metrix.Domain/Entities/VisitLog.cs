namespace Metrix.Domain.Entities;

public class VisitLog
{
    public Guid Id { get; set; }

    public Guid VisitorId { get; set; }
    public Visitor? Visitor { get; set; }

    public DateTime EntryTime { get; set; }

    public DateTime? ExitTime { get; set; }

    public Guid VerifiedBySecurityId { get; set; }
}
