using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Authorization.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email length cannot exceed 100 characters")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50, ErrorMessage = "Password length cannot exceed 50 characters")]
        [MinLength(8, ErrorMessage = "Password length cannot lower than 8 characters")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "CheckPassword is required")]
        [MaxLength(50, ErrorMessage = "CheckPassword length cannot exceed 50 characters")]
        [MinLength(8, ErrorMessage = "CheckPassword length cannot lower than 8 characters")]
        [Compare("Password", ErrorMessage = "Password and CheckPassword are not the same")]
        public string? CheckPassword { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name length cannot exceed 50 characters")]
        public string? Name { get; set; }
    }
}
