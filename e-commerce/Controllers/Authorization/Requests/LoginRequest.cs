using e_commerce.Models;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Authorization.Requests
{
    public class LoginRequest
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessage = "Email不能為空")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;

        /// <summary>
        /// 使用者密碼
        /// </summary>
        [Required(ErrorMessage = "密碼不能為空")]
        public string Password { get; set; } = null!;

        public class LoginRequestExample : IExamplesProvider<LoginRequest>
        {
            public LoginRequest GetExamples()
            {
                return new LoginRequest
                {
                    Email = "user@example.com",
                    Password = "password123"
                };
            }
        }

        public class LoginSuccessResponseExmple : IExamplesProvider<string>
        {
            public string GetExamples()
            {
                return "testToken";
            }
        }

        public class BadRequestResponseExample : IExamplesProvider<ErrorApiResponse>
        {
            public ErrorApiResponse GetExamples()
            {
                return new ErrorApiResponse
                {
                    StatusCode = 401,
                    Message = "帳號或密碼錯誤",
                    Code = 401000,
                    Detail = new { },
                    InnerException = null
                };
            }
        }
    }
}
