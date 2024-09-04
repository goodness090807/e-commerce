using Microsoft.AspNetCore.Http;

namespace e_commerce.Service.Services.Product
{
    public interface IProductService : IBaseService
    {
        /// <summary>
        /// 新增商品基本資料
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="name">商品名稱</param>
        /// <param name="description">商品描述</param>
        /// <param name="price">商品價格</param>
        /// <returns></returns>
        Task<int> AddProductBasicAsync(int userId, string name, string description, decimal price);

        /// <summary>
        /// 新增商品SEO資料
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="productId">商品ID</param>
        /// <param name="metaTitle">SEO網頁名稱</param>
        /// <param name="metaDescription">SEO網頁描述用</param>
        /// <param name="metaPicture">SEO照片</param>
        /// <returns></returns>
        Task AddProductSEOAsync(int userId, int productId, string metaTitle, string metaDescription, IFormFile? metaPicture);
    }
}
