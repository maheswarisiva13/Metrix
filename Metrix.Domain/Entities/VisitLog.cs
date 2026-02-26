namespace Metrix.Domain.Entities;

public class VisitLog
{
    public int Id { get; set; }

    public int VisitorId { get; set; }
    public Visitor? Visitor { get; set; }

    public DateTime EntryTime { get; set; }

    public DateTime? ExitTime { get; set; }

    public int VerifiedBySecurityId { get; set; }
}
