namespace e_commerce.service.Services.Authorization.ViewModels
{
    public class AuthViewModel
    {
        public AuthViewModel(int id, string username, string token, string refreshToken)
        {
            Id = id;
            Username = username;
            Token = token;
            RefreshToken = refreshToken;
        }

        public int Id { get; }
        public string Username { get; }
        public string Token { get; }
        public string RefreshToken { get; }
    }
}
