using e_commerce.Controllers.Product.Requests;
using e_commerce.Extensions;
using e_commerce.Service.Services.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers.Product
{
    /// <summary>
    /// 商品功能
    /// </summary>
    public class ProductsController : BaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// 新增產品基本資訊 (Step 1)
        /// </summary>
        /// <response code="200">新增成功</response>
        [HttpPost, Authorize]
        public async Task<int> AddProductBasicAsync([FromBody] AddProductBasicRequest request)
        {
            return await _productService.AddProductBasicAsync(User.GetUserId(), request.Name, request.Description, request.Price);
        }

        /// <summary>
        /// 新增產品SEO資訊 (Step 2)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [HttpPost("{productId}/SEO"), Authorize]
        public async Task AddProductSEOAsync([FromRoute] int productId, [FromForm] AddProductSEORequest request)
        {
            await _productService.AddProductSEOAsync(User.GetUserId(),
                productId,
                request.MetaTitle,
                request.MetaDescription,
                request.MetaPicture);
        }
    }
}
