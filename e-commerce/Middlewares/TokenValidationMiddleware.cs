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

            var response = new ErrorApiResponse()
            {
                StatusCode = 401,
                Code = 401001,
                Detail = new object(),
            };


            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token))
            {
                response.Message = "Token is missing";
            }
            else if (await _tokenBlacklistService.IsTokenBlacklistedAsync(token))
            {
                response.Message = "Token has been revoked";
            }
            else
            {
                await _next(context);
                return;
            }

            context.Response.StatusCode = response.StatusCode;
            context.Response.ContentType = "application/json";

            await JsonSerializer.SerializeAsync(context.Response.Body, response);
        }
    }
}
