namespace e_commerce.Service.Services.Product
{
    public interface IProductService : IBaseService
    {
        /// <summary>
        /// 新增商品基本資料
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        Task AddProductBasicAsync(int userId, string name, string description, decimal price);

        /// <summary>
        /// 新增商品SEO資料
        /// </summary>
        /// <param name="metaTitle"></param>
        /// <param name="metaDescription"></param>
        /// <returns></returns>
        Task AddProductSEOAsync(string metaTitle, string metaDescription);
    }
}
