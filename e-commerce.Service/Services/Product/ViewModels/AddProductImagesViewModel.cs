namespace e_commerce.Service.Services.Product.ViewModels
{
    public class AddProductImagesViewModel
    {
        public IEnumerable<Image> SuccessImages { get; set; } = new List<Image>();
        public IEnumerable<Fail> FailImages { get; set; } = new List<Fail>();

        public class Image
        {
            public int Id { get; set; }
            public string ImageUrl { get; set; } = string.Empty;
            public bool IsMain { get; set; }
            public int Sort { get; set; }
        }

        public class Fail
        {
            public string FileName { get; set; } = string.Empty;
            public string ErrorMessage { get; set; } = string.Empty;
        }
    }
}
