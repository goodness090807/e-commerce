using e_commerce.Data;
using e_commerce.Data.Enums;
using e_commerce.Service.Services.Product;
using e_commerce.Service.Services.SerialNumber;
using e_commerce.Service.Utils.StorageService;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace e_commerce.Test.Service.Services
{
    public class ProductServiceTests
    {
        private ProductService CreateService(ApplicationDbContext context)
        {
            var serialNumberServiceMock = new Mock<ISerialNumberService>();
            serialNumberServiceMock.Setup(x => x.GenerateSerialNumberAsync(It.IsAny<SerialNumberType>())).ReturnsAsync("SKU12345678");
            var storageService = new Mock<IStorageService>().Object;
            return new ProductService(context, serialNumberServiceMock.Object, storageService); // Use the Mock object
        }

        [Fact(DisplayName = "成功新增商品，回傳商品Id")]
        [Trait("商品服務", "新增商品")]
        public async Task AddProductBasicAsync_Success_ReturnsProductId()
        {
            // Arrange  
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var service = CreateService(context);
                var userId = 1;
                var name = "測試產品";
                var description = "測試產品描述";
                var price = 1000;

                // Act  
                var result = await service.AddProductBasicAsync(userId, name, description, price);

                // Assert  
                Assert.NotEqual(0, result);
            }
        }
    }
}
