using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Authorization.Requests
{
    public class VerifyEmailRequest
    {
        [Required]
        public string Token { get; set; } = null!;
    }
}
