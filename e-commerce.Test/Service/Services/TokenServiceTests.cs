using e_commerce.Common.Utils;
using e_commerce.service.Utils.TokenService;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace e_commerce.Test.Service.Services
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

        /// <summary>
        /// 需要回傳正確的 JWT Token
        /// </summary>
        [Fact]
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

        [Fact]
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
