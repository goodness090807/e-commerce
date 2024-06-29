using e_commerce.Common.Models;
using Microsoft.EntityFrameworkCore.Storage;
using Sentry.Protocol;
using System.Net;
using System.Text.Json;

namespace e_commerce.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "處理請求發生錯誤");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = 500;
            var message = exception.Message;
            var code = 500000;
            var detail = new object();

            if (exception is ApiException apiException)
            {
                statusCode = apiException.StatusCode;
                message = apiException.Message;
                code = apiException.Code;
                detail = apiException.Detail;
            }

            var response = new
            {
                statusCode,
                message,
                code,
                detail,
                innerException = _env.IsDevelopment() ? exception.InnerException?.ToString() : null
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await JsonSerializer.SerializeAsync(context.Response.Body, response);
        }

        private HttpStatusCode GetHttpStatusCode(ApiException apiException)
        {
            return apiException switch
            {
                BadRequestException => HttpStatusCode.BadRequest,
                UnauthorizedException => HttpStatusCode.Unauthorized,
                ForbiddenException => HttpStatusCode.Forbidden,
                NotFoundException => HttpStatusCode.NotFound,
                ConflictException => HttpStatusCode.Conflict,
                UnprocessableEntityException => HttpStatusCode.InternalServerError,
                TooManyRequestsException => HttpStatusCode.InternalServerError,
                InternalServerErrorException => HttpStatusCode.InternalServerError,
                _ => HttpStatusCode.InternalServerError
            };
        }
    }
}
