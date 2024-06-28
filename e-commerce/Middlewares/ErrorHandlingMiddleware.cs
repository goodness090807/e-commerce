using Newtonsoft.Json;
using System.Net;

namespace e_commerce.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHub _sentryHub;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IHub sentryHub)
        {
            _next = next;
            _logger = logger;
            _sentryHub = sentryHub;
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception has occurred while executing the request.");

            // 將異常發送到 Sentry
            _sentryHub.CaptureException(exception);

            var code = HttpStatusCode.InternalServerError;
            var result = JsonConvert.SerializeObject(new { error = "An error occurred while processing your request." });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
