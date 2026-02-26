using Metrix.API.Constants;
using Metrix.Application.DTOs.Admin;
using Metrix.Application.Interfaces.Services;

public class CreateAdminHandler
{
    private readonly IAdminService _adminService;

    public CreateAdminHandler(IAdminService adminService)
    {
        _adminService = adminService;
    }

    public async Task<IResult> Handle(RegisterAdminRequestDto request)
    {
        try
        {
            var message = await _adminService.CreateAdminAsync(request);

            return Results.Created(
                ApiRoutes.Setup.CreateAdmin,
                new { message }
            );
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}