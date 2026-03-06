// NEW FILE → Metrix.Application/DTOs/Request/AdminRequests.cs

using System.ComponentModel.DataAnnotations;

namespace Metrix.Application.DTOs.Admin;

public class CreateSecurityUserRequest
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Email { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}