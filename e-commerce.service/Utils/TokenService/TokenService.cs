using e_commerce.Common.Models;
using e_commerce.Service.Utils.TokenService.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace e_commerce.Service.Utils.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IOptions<AppSettings> options, ILogger<TokenService> logger)
        {
            _appSettings = options.Value;
            _logger = logger;
        }


        public string GenerateJwtToken(string userId, List<Claim>? claims = default, int tokenExpirationMinutes = 0)
        {
            var mergedClaims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, userId),
            };

            if (claims != null)
            {
                mergedClaims.AddRange(claims);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Jwt.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _appSettings.Jwt.Issuer,
                audience: _appSettings.Jwt.Audience,
                claims: mergedClaims,
                expires: DateTimeOffset.UtcNow.DateTime.AddMinutes(tokenExpirationMinutes > 0 ? tokenExpirationMinutes : _appSettings.Jwt.TokenExpirationMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        // refreshToken
        public string RefreshToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Jwt.SecretKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _appSettings.Jwt.Issuer,
                ValidAudience = _appSettings.Jwt.Audience,
                ValidateLifetime = false // Disable token expiration check for refresh token
            };

            SecurityToken validatedToken;
            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
            var jwtToken = validatedToken as JwtSecurityToken;

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedException("Invalid token");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            return GenerateJwtToken(userId);
        }

        public ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Jwt.SecretKey);
            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = _appSettings.Jwt.Issuer,
                    ValidAudience = _appSettings.Jwt.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch (SecurityTokenExpiredException)
            {
                throw new UnauthorizedException("Token expired", code: ErrorCodes.TokenExpired);
            }
            catch(Exception ex)
            {
                _logger.LogWarning(ex, "奇怪的Token進入");
                throw new UnauthorizedException("Invalid token");
            }
        }
    }
}
