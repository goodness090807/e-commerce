using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Authorization.Requests
{
    public class ForgetPasswordRequest
    {
        [Required(ErrorMessage = "信箱為必填欄位")]
        [EmailAddress(ErrorMessage = "信箱格式錯誤")]
        public string Email { get; set; } = null!;
    }
}
