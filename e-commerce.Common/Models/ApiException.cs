namespace e_commerce.Common.Models
{
    public class ApiException : Exception
    {
        public int StatusCode { get; }
        public int Code { get; }
        public object? Detail { get; }

        public ApiException(int statusCode, string message, object? detail = default, int code = 0, Exception? innerException = null) : base(message, innerException)
        {
            StatusCode = statusCode;
            Code = code;
            Detail = detail;
        }
    }

    // BadRequest Exception extends ApiException
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message, object? detail = default, int code = 400000,Exception? innerException = null) 
            : base(400, message, detail, code, innerException)
        {
        }
    }

    // Unauthorized Exception extends ApiException
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message, object? detail = default, int code = 401000, Exception? innerException = null)
            : base(401, message, detail, code, innerException)
        {
        }
    }

    // Forbidden Exception extends ApiException
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message, object? detail = default, int code = 403000, Exception? innerException = null)
            : base(403, message, detail, code, innerException)
        {
        }
    }

    // NotFound Exception extends ApiException
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message, object? detail = default, int code = 404000, Exception? innerException = null)
            : base(404, message, detail, code, innerException)
        {
        }
    }

    // Conflict Exception extends ApiException
    public class ConflictException : ApiException
    {
        public ConflictException(string message, object? detail = default, int code = 409000, Exception? innerException = null)
            : base(409, message, detail, code, innerException)
        {
        }
    }

    // UnprocessableEntity Exception extends ApiException
    public class UnprocessableEntityException : ApiException
    {
        public UnprocessableEntityException(string message, object? detail = default, int code = 422000, Exception? innerException = null)
            : base(422, message, detail, code, innerException)
        {
        }
    }

    // TooManyRequests Exception extends ApiException
    public class TooManyRequestsException : ApiException
    {
        public TooManyRequestsException(string message, object? detail = default, int code = 429000, Exception? innerException = null)
            : base(429, message, detail, code, innerException)
        {
        }
    }

    // InternalServerError Exception extends ApiException
    public class InternalServerErrorException : ApiException
    {
        public InternalServerErrorException(string message, object? detail = default, int code = 500000, Exception? innerException = null) : base(500, message, detail, code, innerException)
        {
        }
    }
}
