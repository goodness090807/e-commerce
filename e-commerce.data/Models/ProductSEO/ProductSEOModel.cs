using e_commerce.Data.Models.Product;

namespace e_commerce.Data.Models.ProductSEO
{
    public class ProductSEOModel : Auditable
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string MetaTitle { get; set; } = string.Empty;
        public string MetaDescription { get; set; } = string.Empty;
        public string MetaPictureUrl { get; set; } = string.Empty;

        public ProductModel Product { get; set; } = null!;
    }
}
