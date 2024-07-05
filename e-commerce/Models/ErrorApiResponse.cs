namespace e_commerce.Models
{
    public class ErrorApiResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Code { get; set; }
        public object? Detail { get; set; }
        public string? InnerException { get; set; }
    }
}
