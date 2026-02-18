namespace Metrix.Application.Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(string userId, string email, string role);
    }
}
