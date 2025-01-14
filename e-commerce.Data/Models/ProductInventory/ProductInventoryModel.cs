using e_commerce.Data.Models.Product;

namespace e_commerce.Data.Models.ProductInventory
{
    /// <summary>
    /// 商品庫存
    /// </summary>
    public class ProductInventoryModel : Auditable
    {
        public int Id { get; set; }
        public int ProductId { get; set; }        
        public int Quantity { get; set; }
        public byte[] RowVersion { get; set; } = null!;


        public ProductModel Product { get; set; } = null!;
    }
}
