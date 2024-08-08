using e_commerce.Data.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Data.Models.Inventory
{
    public class InventoryModel : Auditable
    {
        public int Id { get; set; }
        public int ProductId { get; set; }        
        public int Quantity { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;


        public ProductModel Product { get; set; } = null!;
    }
}
