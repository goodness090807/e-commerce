using e_commerce.Models;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Authorization.Requests
{
    public class RegisterRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(100, ErrorMessage = "Email length cannot exceed 100 characters")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// 姓名(暱稱)
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(50, ErrorMessage = "Name length cannot exceed 50 characters")]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 密碼
        /// </summary>
        [Required(ErrorMessage = "Password is required")]
        [MaxLength(50, ErrorMessage = "Password length cannot exceed 50 characters")]
        [MinLength(8, ErrorMessage = "Password length cannot lower than 8 characters")]
        public string Password { get; set; } = null!;

        /// <summary>
        /// 確認密碼
        /// </summary>
        [Required(ErrorMessage = "CheckPassword is required")]
        [MaxLength(50, ErrorMessage = "CheckPassword length cannot exceed 50 characters")]
        [MinLength(8, ErrorMessage = "CheckPassword length cannot lower than 8 characters")]
        [Compare("Password", ErrorMessage = "Password and CheckPassword are not the same")]
        public string CheckPassword { get; set; } = null!;

        public class RegisterRequestExample : IExamplesProvider<RegisterRequest>
        {
            public RegisterRequest GetExamples()
            {
                return new RegisterRequest
                {
                    Email = "user@example.com",
                    Name = "user",
                    Password = "password123",
                    CheckPassword = "password123"
                };
            }
        }

        public class RegisterSuccessResponseExmple : IExamplesProvider<string>
        {
            public string GetExamples()
            {
                return "testToken";
            }
        }

        public class ConflictResponseExample : IExamplesProvider<ErrorApiResponse>
        {
            public ErrorApiResponse GetExamples()
            {
                return new ErrorApiResponse
                {
                    StatusCode = 409,
                    Message = "信箱已存在",
                    Code = 409000,
                    Detail = new { },
                    InnerException = null
                };
            }
        }
    }
}
