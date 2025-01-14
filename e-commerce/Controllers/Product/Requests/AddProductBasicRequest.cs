using Swashbuckle.AspNetCore.Filters;
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

        public class Example : IExamplesProvider<AddProductBasicRequest>
        {
            public AddProductBasicRequest GetExamples()
            {
                return new AddProductBasicRequest
                {
                    Name = "商品名稱",
                    Description = "商品描述",
                    Price = 100
                };
            }
        }

        public class AddProductSuccessResponseExample : IExamplesProvider<int>
        {
            public int GetExamples()
            {
                return 1;
            }
        }
    }
}
