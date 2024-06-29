using e_commerce.Common.Utils;
using e_commerce.Service.Utils.TokenService;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace e_commerce.Test.Service.Utils
{
    public class TokenServiceTests
    {
        private readonly TokenService _tokenService;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly string _secretKey = KeyGenerator.GenerateRandomKey();

        public TokenServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.SetupGet(x => x["Jwt:SecretKey"]).Returns(_secretKey);
            _configurationMock.SetupGet(x => x["Jwt:Issuer"]).Returns("testIssuer");
            _configurationMock.SetupGet(x => x["Jwt:Audience"]).Returns("testAudience");


            _tokenService = new TokenService(_configurationMock.Object);
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
            Assert.Equal("testIssuer", jwtToken.Issuer);
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
            Assert.Equal("testIssuer", jwtToken.Issuer);
            Assert.Equal(token, jwtToken.RawData);
        }
    }
}
