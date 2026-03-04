using Metrix.API.Handlers;

namespace Metrix.API.Endpoints;

/// <summary>
/// File: Metrix.API/Endpoints/VisitorEndpoints.cs  (NEW FILE)
///
/// Public endpoints — NO auth required.
/// The invitation token in the URL/body identifies the visitor.
///
/// Call app.MapVisitorEndpoints() from Program.cs.
/// </summary>
public static class VisitorEndpoints
{
    public static void MapVisitorEndpoints(this WebApplication app)
    {
        // GET /api/visitor/invite?token=XXXXXXXXXXXXXXXX
        // Called when visitor opens their email link
        app.MapGet("/api/visitor/invite", VisitorHandlers.GetInviteDetailsAsync)
           .WithName("GetInviteDetails")
           .WithTags("Visitor Registration")
           .AllowAnonymous();

        // POST /api/visitor/register
        // Called when visitor submits the registration form
        app.MapPost("/api/visitor/register", VisitorHandlers.RegisterVisitorAsync)
           .WithName("RegisterVisitor")
           .WithTags("Visitor Registration")
           .AllowAnonymous();
    }
}