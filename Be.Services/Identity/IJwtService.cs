using System.Security.Claims;

namespace Be.Services.Identity
{
    public interface IJwtService
    {
        string GenerateAccessToken(long userId, string email, List<string> roles);
        string GenerateAccessToken(long userId, string email, List<string> roles, string phoneNumber);

        string GenerateRefreshToken();

        ClaimsPrincipal GetPrincipalFromToken(string token, bool validateLifeTime = true);
    }
}
