// NEW FILE → Metrix.Application/DTOs/Response/SecurityResponses.cs

namespace Metrix.Application.DTOs.Security;

public class SecurityDashboardDto
{
    public int TodayVisitors { get; set; }
    public int InsideNow { get; set; }
    public int CheckedInToday { get; set; }
    public int CheckedOutToday { get; set; }
    
}

public class SecurityVisitorDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public DateTime VisitDate { get; set; }
    public string IdProofType { get; set; } = string.Empty;
    public string IdProofNumber { get; set; } = string.Empty;
    public string? RegistrationId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string HrName { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? ApprovedByHR { get; set; }
    public DateTime? CheckedInAt { get; set; }
    public DateTime? CheckedOutAt { get; set; }
}

public class VisitLogDto
{
    public int Id { get; set; }
    public string VisitorName { get; set; } = string.Empty;
    public string? RegistrationId { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;  // "CheckIn" | "CheckOut"
    public DateTime EntryTime { get; set; }
    public DateTime? ExitTime { get; set; }
    public string VerifiedBy { get; set; } = string.Empty;
}

public class CheckInOutDto
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Time { get; set; }
}