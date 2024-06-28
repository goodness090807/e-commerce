namespace e_commerce.service.Utils.TokenService
{
    public interface ITokenService
    {
        string GenerateJwtToken(string userId);
        // refreshToken
        string RefreshToken(string token);
        // revokeToken
        void RevokeToken(string token);
    }
}
