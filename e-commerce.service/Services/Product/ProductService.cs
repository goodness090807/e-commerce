using e_commerce.Common.Models;
using e_commerce.Common.Utils;
using e_commerce.Data;
using e_commerce.Data.Enums;
using e_commerce.Data.Models.Product;
using e_commerce.Data.Models.ProductImage;
using e_commerce.Data.Models.ProductSEO;
using e_commerce.Service.Services.Product.ViewModels;
using e_commerce.Service.Services.SerialNumber;
using e_commerce.Service.Utils.StorageService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace e_commerce.Service.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductService> _logger;
        private readonly ISerialNumberService _serialNumberService;
        private readonly IStorageService _storageService;
        private readonly string _folderName = "product";

        public ProductService(ApplicationDbContext context, ILogger<ProductService> logger, ISerialNumberService serialNumberService, IStorageService storageService)
        {
            _context = context;
            _logger = logger;
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

        public async Task AddProductSEOAsync(int userId, int productId, string metaTitle, string metaDescription)
        {
            var product = await GetProductByIdAndUserIdAsync(productId, userId);

            // 只有初次填寫SEO資料時才能填寫，之後請用更新API
            if (product.ProductSEO != null)
            {
                throw new BadRequestException("SEO資料已經填寫過");
            }

            product.ProductSEO = new ProductSEOModel
            {
                MetaTitle = metaTitle,
                MetaDescription = metaDescription,
            };

            await _context.SaveChangesAsync();
        }

        public async Task<AddProductImagesViewModel> AddProductImagesAsync(int userId, int productId, IFormFile[] files)
        {
            var product = await GetProductByIdAndUserIdAsync(productId, userId);

            var productImages = new List<ProductImageModel>();
            var failImages = new List<AddProductImagesViewModel.Fail>();

            foreach (var file in files)
            {
                // 取得檔案相關資訊
                var extension = Path.GetExtension(file.FileName).ToLower();
                var fileName = GeneratorHelper.GetRandomStringByTime(1000);

                // 設定路徑，並上傳檔案
                var filePath = $"{_folderName}/{fileName}{extension}";
                await using var stream = file.OpenReadStream();

                try
                {
                    await _storageService.UploadFileAsync(filePath, file.ContentType, stream);
                    productImages.Add(new()
                    {
                        ProductId = productId,
                        ImageUrl = filePath,
                        IsMain = productImages.Count == 0, // 第一張圖片為主要圖片
                        Sort = productImages.Count + 1
                    });
                }
                catch (Exception e)
                {
                    var errorMessage = $"{file.Name} 上傳失敗";
                    _logger.LogError(e, errorMessage);
                }

                await _context.ProductImages.AddRangeAsync(productImages);
            }

            await _context.SaveChangesAsync();

            return new AddProductImagesViewModel
            {
                SuccessImages = productImages.Select(x => new AddProductImagesViewModel.Image
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    IsMain = x.IsMain,
                    Sort = x.Sort
                }),
                FailImages = failImages
            };
        }

        private async Task<ProductModel> GetProductByIdAndUserIdAsync(int productId, int userId)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == productId && x.UserId == userId);
            if (product == null)
            {
                throw new NotFoundException("找不到商品");
            }
            return product;
        }

        public Task AddProductInventoryAsync(int userId, int productId, int stock, int safetyStock)
        {
            throw new NotImplementedException();
        }
    }
}
