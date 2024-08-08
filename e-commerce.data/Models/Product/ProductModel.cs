using e_commerce.Data.Models.User;

namespace e_commerce.Data.Models.Product
{
    public class ProductModel : Auditable
    {
        public int Id { get; set; }
        /// <summary>
        /// 用戶Id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 商品編號
        /// </summary>
        public string SKU { get; set; } = string.Empty;
        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// 售價
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 是否填寫完成
        /// </summary>
        public bool Done { get; set; }
        /// <summary>
        /// 是否上架
        /// </summary>
        public bool Launched { get; set; }

        public UserModel User { get; set; } = null!;
    }
}
