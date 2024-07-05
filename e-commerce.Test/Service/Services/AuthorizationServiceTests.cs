using e_commerce.Common.Models;
using e_commerce.Data;
using e_commerce.Data.Models.User;
using e_commerce.Service.Services.Authorization;
using e_commerce.Service.Utils.EmailService;
using e_commerce.Service.Utils.TokenBlacklist;
using e_commerce.Service.Utils.TokenService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Moq;

namespace e_commerce.Test.Service.Services
{
    public class AuthorizationServiceTests
    {
        private AuthorizationService CreateService(ApplicationDbContext context)
        {
            var options = Options.Create(new AppSettings());
            var tokenService = new Mock<ITokenService>().Object;
            var emailService = new Mock<IEmailService>().Object;
            var cache = new Mock<IDistributedCache>().Object;
            var tokenBlacklistService = new Mock<ITokenBlacklistService>().Object;
            return new AuthorizationService(context, options, tokenService, emailService, cache, tokenBlacklistService);
        }

        [Fact(DisplayName = "資料庫有Email時但還沒有驗證，回傳false")]
        [Trait("登入授權相關", "Email Check")]
        public async Task CheckEmailAsync_UserExists_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase1")
                .Options;


            using (var context = new ApplicationDbContext(options))
            {
                var service = CreateService(context);
                await context.Users.AddAsync(new UserModel { Email = "test@example.com" });
                await context.SaveChangesAsync();

                // Act
                var result = await service.CheckEmailAsync("test@example.com");

                // Assert
                Assert.False(result);
            }
        }

        [Fact(DisplayName = "資料庫有Email且已驗證，回傳true")]
        [Trait("登入授權相關", "Email Check")]
        public async Task CheckEmailAsync_UserExistsAndValid_ReturnsTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase2")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var service = CreateService(context);
                await context.Users.AddAsync(new UserModel { Email = "test@example.com", Valid = true });
                await context.SaveChangesAsync();

                // Act
                var result = await service.CheckEmailAsync("test@example.com");

                // Assert
                Assert.True(result);
            }
        }

        [Fact(DisplayName = "資料庫沒有Email時，回傳null")]
        [Trait("登入授權相關", "Email Check")]
        public async Task CheckEmailAsync_UserDoesNotExist_ReturnsNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase3")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var service = CreateService(context);

                // Act
                var result = await service.CheckEmailAsync("nonexistent@example.com");

                // Assert
                Assert.Null(result);
            }
        }
    }
}
