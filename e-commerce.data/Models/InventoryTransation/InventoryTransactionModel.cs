using e_commerce.Data.Enums;
using e_commerce.Data.Models.Inventory;

namespace e_commerce.Data.Models.InventoryTransation
{
    public class InventoryTransactionModel
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        /// <summary>
        /// 數量
        /// </summary>
        public int Quantity { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime TransactionDateTime { get; set; }
        /// <summary>
        /// 訂單編號、採購單編號等
        /// </summary>
        public string Reference { get; set; } = string.Empty;
        /// <summary>
        /// 備註
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        public InventoryModel Inventory { get; set; } = new();
    }
}
