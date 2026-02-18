using Metrix.API.Constants;
using Metrix.Application.DTOs.HR;

public class RegisterHrHandler
{
    private readonly IHrService _hrService;

    public RegisterHrHandler(IHrService hrService)
    {
        _hrService = hrService;
    }

    public async Task<IResult> RegisterHr(RegisterHrRequestDto request)
    {
        try
        {
            var id = await _hrService.RegisterHrAsync(request);

            return Results.Created(
                $"{ApiRoutes.Hr.Root}/{id}",
                new { id }
            );
        }
        catch (Exception ex)
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }

}
