using e_commerce.Common.Models;
using e_commerce.Common.Utils;
using e_commerce.Data;
using e_commerce.Data.Models.RefreshToken;
using e_commerce.Data.Models.User;
using e_commerce.Service.Services.Authorization.Models;
using e_commerce.Service.Services.Authorization.ViewModels;
using e_commerce.Service.Utils.EmailService;
using e_commerce.Service.Utils.TokenBlacklist;
using e_commerce.Service.Utils.TokenService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace e_commerce.Service.Services.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IDistributedCache _cache;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        private readonly string _canVerifyKeyPrefix = $"verify_email_token";
        private readonly string _resetPassowrdKeyPrefix = $"reset_password";

        public AuthorizationService(ApplicationDbContext context, IOptions<AppSettings> options
            , ITokenService tokenService, IEmailService emailService, IDistributedCache cache
            , ITokenBlacklistService tokenBlacklistService)
        {
            _context = context;
            _appSettings = options.Value;
            _tokenService = tokenService;
            _emailService = emailService;
            _cache = cache;
            _tokenBlacklistService = tokenBlacklistService;
        }

        public async Task<bool?> CheckEmailAsync(string email)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

            if (user == null)
            {
                return null;
            }

            return user.Valid;
        }

        public async Task<AuthViewModel> LoginAsync(string email, string password)
        {
            var user = await _context.Users.Include(x => x.RefreshToken).FirstOrDefaultAsync(x => x.Email == email);
            if (user == null || !HashHelper.VerifyPassword(password, user.HashedPassword))
            {
                throw new UnauthorizedException("帳號或密碼錯誤");
            }

            if (!user.Valid)
            {
                throw new UnauthorizedException("帳號未驗證", code: ErrorCodes.AccountNotValid);
            }

            // TODO：送出登入的通知信件(待優化)
            string subject = "Login Notification";
            string body = "You are Login. Is this you?";
            await _emailService.SendEmailAsync(user.Email, subject, body);

            return await AddOrResetTokenAsync(user);
        }

        public async Task RegisterAsync(string email, string password, string name)
        {
            if (await _context.Users.AsNoTracking().AnyAsync(x => x.Email == email))
            {
                throw new ConflictException("信箱已存在");
            }

            var user = new UserModel
            {
                Email = email,
                HashedPassword = HashHelper.HashPassword(password),
                Name = name,
            };

            using var transaction = await _context.Database.BeginTransactionAsync();

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            await SendVerificationEmailAsync(user);

            await transaction.CommitAsync();
        }

        public async Task VerifyEmailAsync(string token)
        {
            var principal = _tokenService.GetPrincipalFromToken(token);
            if (principal == null)
            {
                throw new BadRequestException("Invalid token.");
            }

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;

            var cacheToken = await _cache.GetStringAsync($"{_canVerifyKeyPrefix}_{email}");
            if (cacheToken == null || cacheToken != token)
            {
                throw new UnauthorizedException("Invalid token.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            user.Valid = true;
            await _context.SaveChangesAsync();

            // TODO：送出歡迎的信件(待優化)
            string subject = "Welcome to e-commerce";
            string body = "Welcome to e-commerce!";
            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        public async Task ResendVerificationEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            if (user.Valid)
            {
                throw new BadRequestException("User already verified.");
            }

            await CheckMailSendingFrequencyAsync($"resend_verify_email_frequency_{email}");
            await SendVerificationEmailAsync(user);
        }

        public async Task<AuthViewModel> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
            {
                throw new UnauthorizedException("Refresh token 不能為空");
            }

            var token = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (token == null || token.ExpiredAt < DateTimeHelper.GetUTC8Now())
            {
                throw new UnauthorizedException("Refresh token 過期");
            }

            var user = await _context.Users.Include(x => x.RefreshToken).FirstOrDefaultAsync(x => x.Id == token.UserId);
            if (user == null)
            {
                throw new UnauthorizedException("使用者不存在");
            }

            return await AddOrResetTokenAsync(user);
        }

        public async Task RevokeTokenAsync(string? token, string? refreshToken)
        {
            if (!string.IsNullOrEmpty(token))
            {
                await _tokenBlacklistService.BlacklistTokenAsync(token);
            }

            var refreshTokenModel = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.RefreshToken == refreshToken);
            if (refreshTokenModel != null)
            {
                _context.RefreshTokens.Remove(refreshTokenModel);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ForgetPasswordAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            await CheckMailSendingFrequencyAsync($"forget_password_frequency_{email}");
            await SendForgetPasswordAsync(user);
        }

        public async Task ResetPasswordAsync(string token, string password)
        {
            var principal = _tokenService.GetPrincipalFromToken(token);
            if (principal == null)
            {
                throw new BadRequestException("Invalid token.");
            }

            var email = principal.FindFirst(ClaimTypes.Email)?.Value;

            var cacheToken = await _cache.GetStringAsync($"{_resetPassowrdKeyPrefix}_{email}");
            if (cacheToken == null || cacheToken != token)
            {
                throw new UnauthorizedException("Invalid token.");
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if (user == null)
            {
                throw new NotFoundException("User not found.");
            }

            user.HashedPassword = HashHelper.HashPassword(password);
            // 將RefreshToken移除，意味著舊的Token將失效
            user.RefreshToken = null;
            await _context.SaveChangesAsync();

            await _cache.RemoveAsync($"{_resetPassowrdKeyPrefix}_{email}");
        }

        private async Task<AuthViewModel> AddOrResetTokenAsync(UserModel user)
        {
            var token = _tokenService.GenerateJwtToken(user.Id.ToString());
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = new RefreshTokenModel
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                ExpiredAt = DateTimeHelper.GetUTC8Now().AddDays(_appSettings.Jwt.RefreshTokenExpirationDays),
            };

            await _context.SaveChangesAsync();

            return new(token, refreshToken);
        }        

        private async Task SendVerificationEmailAsync(UserModel user)
        {
            var token = _tokenService.GenerateJwtToken(user.Id.ToString(),
                new ()
                {
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.Name),
                },
                _appSettings.Verification.TokenExpirationMinutes);

            await _cache.SetStringAsync($"{_canVerifyKeyPrefix}_{user.Email}", token, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_appSettings.Verification.TokenExpirationMinutes),
            });

            var callbackUrl = $"{_appSettings.Verification.BaseUrl}/verify-email?token={token}";

            string subject = "Confirm your email";
            string body = $"Please confirm your account by <a href='{callbackUrl}'>clicking here</a>.";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        private async Task SendForgetPasswordAsync(UserModel user)
        {
            var token = _tokenService.GenerateJwtToken(user.Id.ToString(),
                new()
                {
                    new(ClaimTypes.Email, user.Email),
                    new(ClaimTypes.Name, user.Name),
                },
                _appSettings.Verification.TokenExpirationMinutes);

            await _cache.SetStringAsync($"{_resetPassowrdKeyPrefix}_{user.Email}", token, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_appSettings.Verification.TokenExpirationMinutes),
            });

            var callbackUrl = $"{_appSettings.Verification.BaseUrl}/reset-password?token={token}";

            string subject = "Reset your password";
            string body = $"Please reset your password by <a href='{callbackUrl}'>clicking here</a>.";

            await _emailService.SendEmailAsync(user.Email, subject, body);
        }

        /// <summary>
        /// 檢查Email發送頻率(使用redis cache來限制發送驗證信的頻率，當一分鐘內發送超過三次時，則拒絕發送)
        /// </summary>
        /// <param name="key">cache key，搭配Eamil變成唯一值</param>
        /// <exception cref="BadRequestException">傳送email太過頻繁</exception>
        private async Task CheckMailSendingFrequencyAsync(string key)
        {
            var count = await _cache.GetStringAsync(key);
            if (count != null && int.Parse(count) >= 3)
            {
                throw new BadRequestException("Resend email too frequently.");
            }

            var newCount = count == null ? "1" : (int.Parse(count) + 1).ToString();

            await _cache.SetStringAsync(key, newCount, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1),
            });
        }
    }
}
