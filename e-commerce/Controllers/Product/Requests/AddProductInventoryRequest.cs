namespace e_commerce.Controllers.Product.Requests
{
    public class AddProductInventoryRequest
    {
        public int Stock { get; set; }

        public int SafetyStock { get; set; }
    }
}
