using e_commerce.Common.Models;
using e_commerce.Data;
using e_commerce.Data.Models.Product;
using e_commerce.Data.Models.User;

namespace e_commerce.Service.Services.Product
{
    internal class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductBasicAsync(int userId, string name, string description, decimal price)
        {
            if (!await _context.Users.CheckValidUserAsync(userId))
            {
                throw new UnauthorizedException("User not found");
            }

            // TODO: Add SKU

            var product = new ProductModel
            {
                Name = name,
                Description = description,
                Price = price,
                UserId = userId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddProductSEOAsync(string metaTitle, string metaDescription)
        {
            throw new NotImplementedException();
        }
    }
}
