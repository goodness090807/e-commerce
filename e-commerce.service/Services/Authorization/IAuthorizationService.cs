using e_commerce.Service.Services.Authorization.ViewModels;

namespace e_commerce.Service.Services.Authorization
{
    public interface IAuthorizationService : IBaseService
    {
        Task<bool?> CheckEmailAsync(string email);
        Task<AuthViewModel> LoginAsync(string email, string password);
        Task RegisterAsync(string email, string password, string name);
        Task VerifyEmailAsync(string token);
        Task ResendVerificationEmailAsync(string email);
        Task<AuthViewModel> RefreshTokenAsync(string refreshToken);
        Task RevokeTokenAsync(string? token, string? refreshToken);
        Task ForgetPasswordAsync(string email);
        Task ResetPasswordAsync(string token, string password);
    }
}
