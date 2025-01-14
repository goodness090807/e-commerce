using e_commerce.Controllers.Product.Requests;
using e_commerce.Extensions;
using e_commerce.Models;
using e_commerce.Service.Services.Product;
using e_commerce.Service.Services.Product.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

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
        [SwaggerRequestExample(typeof(AddProductBasicRequest), typeof(AddProductBasicRequest.Example))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AddProductBasicRequest.AddProductSuccessResponseExample))]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [HttpPost, Authorize]
        public async Task<int> AddProductBasicAsync([FromBody] AddProductBasicRequest request)
        {
            return await _productService.AddProductBasicAsync(User.GetUserId(), request.Name, request.Description, request.Price);
        }

        /// <summary>
        /// 新增產品SEO資訊 (Step 2)
        /// </summary>
        /// <response code="200">新增成功</response>
        [SwaggerRequestExample(typeof(AddProductSEORequest), typeof(AddProductSEORequest.Example))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(AddProductSEORequest.BadRequestResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(AddProductSEORequest.NotFoundResponseExample))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorApiResponse))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorApiResponse))]
        [HttpPost("{productId}/SEO"), Authorize]
        public async Task AddProductSEOAsync([FromRoute] int productId, [FromBody] AddProductSEORequest request)
        {
            await _productService.AddProductSEOAsync(User.GetUserId(),
                productId,
                request.MetaTitle,
                request.MetaDescription);
        }

        /// <summary>
        /// 新增商品圖片 (Step 3)
        /// </summary>
        /// <response code="200">新增完成，並回傳成功與失敗的結果</response>
        [HttpPost("{productId}/images"), Authorize]
        public async Task<AddProductImagesViewModel> AddProductImagesAsync([FromRoute] int productId, [FromForm] AddProductImagesRequest request)
        {
            return await _productService.AddProductImagesAsync(User.GetUserId(), productId, request.Images);
        }

        /// <summary>
        /// 新增商品庫存 (Step 4)
        /// </summary>
        [HttpPost("{productId}/inventory"), Authorize]
        public async Task AddProductInventoryAsync([FromRoute] int productId, [FromBody] AddProductInventoryRequest request)
        {
            await _productService.AddProductInventoryAsync(User.GetUserId(), productId, request.Stock, request.SafetyStock);
        }
    }
}
