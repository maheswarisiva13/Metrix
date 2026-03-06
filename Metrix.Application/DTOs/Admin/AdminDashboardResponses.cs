using System;
using System.Collections.Generic;
using System.Text;

namespace Metrix.Application.DTOs.Admin;
public class AdminDashboardDto
{
    public int TotalSecurityUsers { get; set; }
    public int ActiveSecurityUsers { get; set; }
    public int TotalVisitors { get; set; }
    public int ApprovedVisitors { get; set; }
    public int CheckedInCount { get; set; }
    public int CheckedOutCount { get; set; }
}

public class SecurityUserDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class AdminVisitorDto
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

