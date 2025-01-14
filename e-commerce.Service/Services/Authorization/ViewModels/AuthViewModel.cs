namespace e_commerce.Service.Services.Authorization.ViewModels
{
    public class AuthViewModel
    {
        public AuthViewModel(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// 存取用 Token
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// 刷新用 Token
        /// </summary>
        public string RefreshToken { get; }
    }
}
