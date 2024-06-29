namespace e_commerce.Service.Utils.TokenService
{
    public interface ITokenService : IBaseService
    {
        string GenerateJwtToken(string userId);
        string GenerateRefreshToken();
        // refreshToken
        string RefreshToken(string token);
        // revokeToken
        void RevokeToken(string token);
    }
}
