using e_commerce.Models;
using Swashbuckle.AspNetCore.Filters;

namespace e_commerce.Controllers.Product.Requests
{
    public class AddProductSEORequest
    {
        public string MetaTitle { get; set; } = string.Empty;

        public string MetaDescription { get; set; } = string.Empty;

        public class Example : IExamplesProvider<AddProductSEORequest>
        {
            public AddProductSEORequest GetExamples()
            {
                return new AddProductSEORequest
                {
                    MetaTitle = "在外部看到的標題",
                    MetaDescription = "在外部看到的商品描述，可以很長"
                };
            }
        }

        public class BadRequestResponseExample : IExamplesProvider<ErrorApiResponse>
        {
            public ErrorApiResponse GetExamples()
            {
                return new ErrorApiResponse
                {
                    Code = 400,
                    Message = "找不到商品"
                };
            }
        }

        public class NotFoundResponseExample : IExamplesProvider<ErrorApiResponse>
        {
            public ErrorApiResponse GetExamples()
            {
                return new ErrorApiResponse
                {
                    Code = 404,
                    Message = "找不到商品"
                };
            }
        }
    }
}
