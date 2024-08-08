using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Product.Requests
{
    public class AddProductBasicRequest
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }
    }
}
