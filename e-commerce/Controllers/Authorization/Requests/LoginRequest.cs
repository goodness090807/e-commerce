namespace e_commerce.Controllers.Authorization.Requests
{
    public class LoginRequest
    {
        // / <summary>
        // / 使用者帳號
        // / </summary>
        public string Account { get; set; }

        // / <summary>
        // / 使用者密碼
        // / </summary>
        public string Password { get; set; }

    }
}
