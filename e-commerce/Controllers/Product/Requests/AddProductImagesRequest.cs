using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Product.Requests
{
    public class AddProductImagesRequest : IValidatableObject
    {
        private readonly string[] _allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".svg", ".gif" };
        private readonly int _maxFileSize = 5 * 1024 * 1024;

        public IFormFile[] Images { get; set; } = Array.Empty<IFormFile>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            foreach (var image in Images)
            {
                if (image.Length > _maxFileSize)
                {
                    yield return new ValidationResult($"{image.FileName} 圖片大小超過 5MB");
                }

                var extension = Path.GetExtension(image.FileName);
                if (!_allowedExtensions.Contains(extension.ToLower()))
                {
                    yield return new ValidationResult($"{image.FileName} 圖片格式錯誤");
                }
            }
        }
    }
}
