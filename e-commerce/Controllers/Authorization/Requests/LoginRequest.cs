using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Authorization.Requests
{
    public class LoginRequest
    {
        // / <summary>
        // / 使用者帳號
        // / </summary>
        [Required(ErrorMessage = "Email不能為空")]
        public string Email { get; set; } = string.Empty;

        // / <summary>
        // / 使用者密碼
        // / </summary>
        [Required(ErrorMessage = "密碼不能為空")]
        public string Password { get; set; } = string.Empty;

    }
}
