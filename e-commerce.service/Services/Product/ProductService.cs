using e_commerce.Common.Models;
using e_commerce.Common.Utils;
using e_commerce.Data;
using e_commerce.Data.Enums;
using e_commerce.Data.Models.Product;
using e_commerce.Data.Models.ProductSEO;
using e_commerce.Service.Services.SerialNumber;
using e_commerce.Service.Utils.StorageService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Service.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ISerialNumberService _serialNumberService;
        private readonly IStorageService _storageService;
        private readonly string _folderName = "product";

        public ProductService(ApplicationDbContext context, ISerialNumberService serialNumberService, IStorageService storageService)
        {
            _context = context;
            _serialNumberService = serialNumberService;
            _storageService = storageService;
        }

        public async Task<int> AddProductBasicAsync(int userId, string name, string description, decimal price)
        {
            var sku = await _serialNumberService.GenerateSerialNumberAsync(SerialNumberType.SKU);

            var product = new ProductModel
            {
                SKU = sku,
                Name = name,
                Description = description,
                Price = price,
                UserId = userId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return product.Id;
        }

        public async Task AddProductSEOAsync(int userId, int productId, string metaTitle, string metaDescription, IFormFile? metaPicture)
        {
            var product = _context.Products.Include(x => x.ProductSEO).FirstOrDefault(x => x.Id == productId && x.UserId == userId);

            if (product == null)
            {
                throw new NotFoundException("找不到商品");
            }

            // 只有初次填寫SEO資料時才能填寫，之後請用更新API
            if (product.ProductSEO != null)
            {
                throw new BadRequestException("SEO資料已經填寫過");
            }

            var filePath = string.Empty;
            if (metaPicture != null)
            {
                // 取得檔案相關資訊
                var extension = Path.GetExtension(metaPicture.FileName);
                var fileName = GeneratorHelper.GetRandomStringByTime(1000);

                // 設定路徑，並上傳檔案
                filePath = $"{_folderName}/{fileName}{extension}";
                await using var stream = metaPicture.OpenReadStream();
                await _storageService.UploadFileAsync(filePath, metaPicture.ContentType, stream);
            }

            product.ProductSEO = new ProductSEOModel
            {
                MetaTitle = metaTitle,
                MetaDescription = metaDescription,
                MetaPictureUrl = filePath
            };

            await _context.SaveChangesAsync();
        }
    }
}
