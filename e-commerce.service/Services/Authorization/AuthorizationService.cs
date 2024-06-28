using e_commerce.Common.Models;
using e_commerce.Common.Utils;
using e_commerce.data;
using e_commerce.data.Models.User;
using e_commerce.service.Services.Authorization.ViewModels;
using e_commerce.service.Utils.TokenService;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.service.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AuthorizationService(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            return user != null;
        }

        public async Task<AuthViewModel> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.HashedPassword == HashHelper.HashPassword(password));
            if (user == null)
            {
                throw new UnauthorizedException("帳號或密碼錯誤");
            }

            // Generate token
            var token = _tokenService.GenerateJwtToken(user.Id.ToString());
            // Generate refresh token
            var refreshToken = _tokenService.GenerateRefreshToken();

            return new AuthViewModel(user.Id, user.Name, token, refreshToken);
        }

        public async Task<AuthViewModel> Register(string email, string password, string name)
        {
            // Check if account exists
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user != null)
            {
                throw new ConflictException("信箱已存在");
            }

            // Create new user
            user = new UserModel
            {
                Email = email,
                HashedPassword = HashHelper.HashPassword(password),
                Name = name,
            };

            await _context.SaveChangesAsync();

            // Generate token
            var token = _tokenService.GenerateJwtToken(user.Id.ToString());
            // Generate refresh token
            var refreshToken = _tokenService.GenerateRefreshToken();

            return new AuthViewModel(user.Id, user.Name, token, refreshToken);
        }
    }
}
