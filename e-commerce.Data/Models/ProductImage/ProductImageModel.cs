using e_commerce.Data.Models.Product;

namespace e_commerce.Data.Models.ProductImage
{
    public class ProductImageModel : Auditable
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        /// <summary>
        /// 圖片連結
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// 圖片標題
        /// </summary>
        public string ImageTitle { get; set; } = string.Empty;

        /// <summary>
        /// 是否為主要圖片
        /// </summary>
        public bool IsMain { get; set; } = false;

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        public ProductModel Product { get; set; } = null!;
    }
}
