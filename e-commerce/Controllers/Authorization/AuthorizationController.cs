using e_commerce.Common.Models;
using e_commerce.Common.Utils;
using e_commerce.Controllers.Authorization.Requests;
using e_commerce.Models;
using e_commerce.Service.Services.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Controllers.Authorization
{
    public class AuthorizationController : BaseController
    {
        private readonly AppSettings _appSettings;
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IOptions<AppSettings> options, IAuthorizationService authorizationService)
        {
            _appSettings = options.Value;
            _authorizationService = authorizationService;
        }

        /// <summary>
        /// 確認信箱是否註冊過
        /// </summary>
        /// <response code="200">
        /// 結果：
        /// - true：信箱存在，且已驗證
        /// - false：信箱存在，但還未驗證
        /// - null：信箱不存在
        /// </response>
        [HttpGet("check")]
        public async Task<bool?> CheckEmailAsync([FromQuery][EmailAddress(ErrorMessage = "信箱格式錯誤")] string email)
        {
            return await _authorizationService.CheckEmailAsync(email);
        }

        /// <summary>
        /// 使用者登入
        /// </summary>
        /// <response code="200">回傳Token</response>
        /// <response code="401">
        /// 可能的錯誤情況：
        /// - 401000：帳號或密碼錯誤
        /// - 401001：帳號未驗證
        /// </response>
        [HttpPost("login")]
        [SwaggerRequestExample(typeof(LoginRequest), typeof(LoginRequest.LoginRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoginRequest.LoginSuccessResponseExmple))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(LoginRequest.BadRequestResponseExample))]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorApiResponse), StatusCodes.Status401Unauthorized)]
        public async Task<string> LoginAsync([FromBody] LoginRequest request)
        {
            var res = await _authorizationService.LoginAsync(request.Email, request.Password);

            SetRefreshTokenCookie(res.RefreshToken);

            return res.Token;
        }

        /// <summary>
        /// 使用者註冊
        /// </summary>
        /// <response code="200">註冊成功，且發出驗證信</response>
        /// <response code="409">信箱已存在</response>
        [SwaggerRequestExample(typeof(RegisterRequest), typeof(RegisterRequest.RegisterRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RegisterRequest.RegisterRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(RegisterRequest.ConflictResponseExample))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(ErrorApiResponse))]
        [HttpPost("register")]
        public async Task RegisterAsync([FromBody] RegisterRequest request)
        {
            await _authorizationService.RegisterAsync(request.Email, request.Password, request.Name);
        }

        /// <summary>
        /// 帳號驗證
        /// </summary>
        [HttpPost("verify-email")]
        public async Task VerifyEmailAsync([FromBody] VerifyEmailRequest request)
        {
            await _authorizationService.VerifyEmailAsync(request.Token);
        }

        /// <summary>
        /// 重新發送驗證信
        /// </summary>
        [HttpPost("resend-verification-email")]
        public async Task ResendVerificationEmailAsync([FromBody] ResendVerifyEmailRequest request)
        {
            await _authorizationService.ResendVerificationEmailAsync(request.Email);
        }

        /// <summary>
        /// 刷新 Token
        /// </summary>
        /// <response code="200">回傳token，並重新設置Refresh Token</response>
        [HttpGet("refresh-token")]
        public async Task<string> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedException("Refresh token 不能為空");
            }

            var res = await _authorizationService.RefreshTokenAsync(refreshToken);

            SetRefreshTokenCookie(res.RefreshToken);

            return res.Token;
        }

        /// <summary>
        /// 撤銷Token
        /// </summary>
        /// <response code="204">回傳token，並重新設置Refresh Token</response>
        [HttpDelete("revoke-token"), Microsoft.AspNetCore.Authorization.Authorize]
        public async Task RevokeTokenAsync()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ")[1];
            var refreshToken = Request.Cookies["RefreshToken"];

            await _authorizationService.RevokeTokenAsync(token, refreshToken);

            Response.Cookies.Delete("RefreshToken");
        }

        /// <summary>
        /// 忘記密碼
        /// </summary>
        [HttpPost("forget-password")]
        public async Task ForgetPasswordAsync([FromBody] ForgetPasswordRequest request)
        {
            await _authorizationService.ForgetPasswordAsync(request.Email);
        }

        /// <summary>
        /// 重設密碼
        /// </summary>
        [HttpPut("reset-password")]
        public async Task ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            await _authorizationService.ResetPasswordAsync(request.Token, request.Password);
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // 允許跨站請求
                Expires = DateTimeHelper.GetUTC8Now().AddDays(_appSettings.Jwt.RefreshTokenExpirationDays)
            });
        }
    }
}
