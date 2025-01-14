using System.Security.Claims;

namespace e_commerce.Service.Utils.TokenService
{
    public interface ITokenService : IBaseService
    {
        string GenerateJwtToken(string userId, List<Claim>? claims = default, int tokenExpirationMinutes = 0);
        string GenerateRefreshToken();
        string RefreshToken(string token);
        ClaimsPrincipal GetPrincipalFromToken(string token);
    }
}
