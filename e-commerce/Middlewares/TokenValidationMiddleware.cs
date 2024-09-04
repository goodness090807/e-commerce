using e_commerce.Models;
using e_commerce.Service.Utils.TokenBlacklist;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace e_commerce.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenBlacklistService _tokenBlacklistService;

        public TokenValidationMiddleware(RequestDelegate next, ITokenBlacklistService tokenBlacklistService)
        {
            _next = next;
            _tokenBlacklistService = tokenBlacklistService;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<AuthorizeAttribute>() == null)
            {
                await _next(context);
                return;
            }

            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (string.IsNullOrEmpty(token))
                {
                    await SetUnauthorizedResponse(context, "Token不存在");
                    return;
                }
                
                if (await _tokenBlacklistService.IsTokenBlacklistedAsync(token))
                {
                    await SetUnauthorizedResponse(context, "Token已被移除");
                    return;
                }
            }
            catch(Exception ex)
            {
                await SetUnauthorizedResponse(context, ex.Message);
                return;
            }

            await _next(context);
        }

        private async Task SetUnauthorizedResponse(HttpContext context, string errorMessage)
        {
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var response = new ErrorApiResponse()
            {
                StatusCode = 401,
                Code = 401001,
                Message = errorMessage,
            };

            await JsonSerializer.SerializeAsync(context.Response.Body, response);
        }
    }
}
