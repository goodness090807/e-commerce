using e_commerce.Common.Models;
using e_commerce.Middlewares;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System.Net;

namespace e_commerce.Test.API.Middlewares
{
    /// <summary>
    /// ErrorHandlingMiddleware測試
    /// </summary>
    [Trait("Middleware", "錯誤處理")]
    public class ErrorHandlingMiddlewareTests
    {
        private readonly Mock<ILogger<ErrorHandlingMiddleware>> _loggerMock;
        private readonly Mock<IWebHostEnvironment> _envMock;

        public ErrorHandlingMiddlewareTests()
        {
            _loggerMock = new Mock<ILogger<ErrorHandlingMiddleware>>();
            _envMock = new Mock<IWebHostEnvironment>();
        }

        [Fact(DisplayName = "當進入Middleware時，確保呼叫一次Delegate")]
        public async Task Invoke_Should_CallNextDelegate()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var nextDelegateMock = new Mock<RequestDelegate>();
            var middleware = new ErrorHandlingMiddleware(nextDelegateMock.Object, _loggerMock.Object, _envMock.Object);

            // Act
            await middleware.Invoke(context);

            // Assert
            nextDelegateMock.Verify(next => next(context), Times.Once);
        }

        [Fact(DisplayName = "當拋出一般的例外時，預期必須回傳 500 Status Code")]
        public async Task Invoke_Should_HandleExceptionAndReturnInternalServerError()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var nextDelegateMock = new Mock<RequestDelegate>();
            nextDelegateMock.Setup(next => next(context)).Throws(new Exception("Test exception"));

            var middleware = new ErrorHandlingMiddleware(nextDelegateMock.Object, _loggerMock.Object, _envMock.Object);

            // Act
            await middleware.Invoke(context);

            // 重設 MemoryStream 的位置，以便能夠從頭讀取
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Equal("application/json", context.Response.ContentType);

            var responseContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
            var apiException = JsonConvert.DeserializeObject<ApiException>(responseContent);

            Assert.NotNull(apiException);
            Assert.Equal((int)HttpStatusCode.InternalServerError, apiException.StatusCode);
            Assert.Equal("Test exception", apiException.Message);
            Assert.Equal(500000, apiException.Code);
        }

        [Fact(DisplayName = "當收到BadRequestException時，預期必須回傳 400 Status Code")]
        public async Task Invoke_Should_HandleApiExceptionAndReturnCorrectStatusCode()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            var nextDelegateMock = new Mock<RequestDelegate>();
            nextDelegateMock.Setup(next => next(context)).Throws(new BadRequestException("Bad request"));

            var middleware = new ErrorHandlingMiddleware(nextDelegateMock.Object, _loggerMock.Object, _envMock.Object);

            // Act
            await middleware.Invoke(context);

            // Assert
            Assert.Equal(400, context.Response.StatusCode);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var responseBody = await reader.ReadToEndAsync();
            Assert.Contains("Bad request", responseBody);
        }
    }
}
