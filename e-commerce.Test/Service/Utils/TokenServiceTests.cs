using e_commerce.Common.Models;
using e_commerce.Service.Utils.TokenService;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static e_commerce.Common.Models.AppSettings;

namespace e_commerce.Test.Service.Utils
{
    [Trait("Token相關", "驗證Token產生正確性")]
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly AppSettings _appSettings;

        public TokenServiceTests()
        {
            _appSettings = new AppSettings
            {
                Jwt = new JwtSettings
                {
                    SecretKey = "YourTestSecretKeyHereMustBeLongEnough",
                    Issuer = "TestIssuer",
                    Audience = "TestAudience",
                    TokenExpirationMinutes = 60
                }
            };
            var options = Options.Create(_appSettings);

            // Mock一個Ilogger
            var logger = new Mock<ILogger<TokenService>>().Object;

            _tokenService = new TokenService(options, logger);
        }

        [Fact(DisplayName = "產生JWT Token時，驗證是合法的Token")]
        public void GenerateJwtToken_ShouldReturnValidToken()
        {
            // Arrange
            var userId = "testUserId";

            // Act
            var token = _tokenService.GenerateJwtToken(userId);

            // Assert
            Assert.NotNull(token);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Equal(userId, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
        }

        [Fact(DisplayName = "刷新Token時，需要回傳合法的Token")]
        public void RefreshToken_ShouldReturnValidToken()
        {
            // Arrange
            var userId = "testUserId";

            // Act
            var token = _tokenService.GenerateJwtToken(userId);

            // Act
            var refreshedToken = _tokenService.RefreshToken(token);

            // Assert
            Assert.NotNull(refreshedToken);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            Assert.Equal("TestIssuer", jwtToken.Issuer);
            Assert.Equal(token, jwtToken.RawData);
        }
    }
}
