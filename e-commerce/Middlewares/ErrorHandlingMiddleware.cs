using e_commerce.Common.Models;
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = exception.Message;
            var errorCode = 500000;

            if (exception is ApiException apiException)
            {
                code = GetHttpStatusCode(apiException);
                message = apiException.Message;
                errorCode = apiException.Code;
            }

            var result = JsonSerializer.Serialize(new ApiException((int)code, message, errorCode));

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
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
