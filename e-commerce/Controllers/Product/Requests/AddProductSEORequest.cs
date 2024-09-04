using e_commerce.Attributes;
using e_commerce.Common.Models;

namespace e_commerce.Controllers.Product.Requests
{
    public class AddProductSEORequest
    {
        public string MetaTitle { get; set; } = string.Empty;

        public string MetaDescription { get; set; } = string.Empty;

        [MaxFileSize(5 * 1024 * 1024)]
        [AllowedExtensions([
            FileExtensions.JPG,
            FileExtensions.JPEG,
            FileExtensions.PNG,
            FileExtensions.SVG,
            FileExtensions.WEBP])]
        public IFormFile? MetaPicture { get; set; }
    }
}
