using e_commerce.Common.Models;
using e_commerce.Models;
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = new ErrorApiResponse()
            {
                StatusCode = 500,
                Message = exception.Message,
                Code = 500000,
                Detail = new object(),
                InnerException = _env.IsDevelopment() ? exception.InnerException?.ToString() : null
            };

            if (exception is ApiException apiException)
            {
                response.StatusCode = apiException.StatusCode;
                response.Message = apiException.Message;
                response.Code = apiException.Code;
                response.Detail = apiException.Detail;
            }
            else
            {
                _logger.LogError(exception, response.Message, exception.InnerException);
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;

            await JsonSerializer.SerializeAsync(context.Response.Body, response);
        }
    }
}
