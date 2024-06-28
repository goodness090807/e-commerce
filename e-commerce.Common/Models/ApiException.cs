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
            Code = code == 0 ? GetDefaultCode(statusCode) : code;
            Detail = detail;
        }

        private static int GetDefaultCode(int statusCode)
        {
            return statusCode switch
            {
                400 => 400000,
                401 => 401000,
                403 => 403000,
                404 => 404000,
                409 => 409000,
                422 => 422000,
                429 => 429000,
                500 => 500000,
                501 => 501000,
                502 => 502000,
                503 => 503000,
                504 => 504000,
                _ => 0
            };
        }
    }

    // BadRequest Exception extends ApiException
    public class BadRequestException : ApiException
    {
        public BadRequestException(string message, object? detail = default, int code = 0, Exception? innerException = null) : base(400, message, detail, code, innerException)
        {
        }
    }

    // Unauthorized Exception extends ApiException
    public class UnauthorizedException : ApiException
    {
        public UnauthorizedException(string message, object? detail = default, int code = 0, Exception? innerException = null) : base(401, message, detail, code, innerException)
        {
        }
    }

    // Forbidden Exception extends ApiException
    public class ForbiddenException : ApiException
    {
        public ForbiddenException(string message, object? detail = default, int code = 0, Exception? innerException = null) : base(403, message, detail, code, innerException)
        {
        }
    }

    // NotFound Exception extends ApiException
    public class NotFoundException : ApiException
    {
        public NotFoundException(string message, object? detail = default, int code = 0, Exception? innerException = null) : base(404, message, detail, code, innerException)
        {
        }
    }

    // Conflict Exception extends ApiException
    public class ConflictException : ApiException
    {
        public ConflictException(string message, object? detail = default, int code = 0, Exception? innerException = null) : base(409, message, detail, code, innerException)
        {
        }
    }

    // UnprocessableEntity Exception extends ApiException
    public class UnprocessableEntityException : ApiException
    {
        public UnprocessableEntityException(string message, object? detail = default, int code = 0, Exception? innerException = null) : base(422, message, detail, code, innerException)
        {
        }
    }

    // TooManyRequests Exception extends ApiException
    public class TooManyRequestsException : ApiException
    {
        public TooManyRequestsException(string message, object? detail = default, int code = 0, Exception? innerException = null) : base(429, message, detail, code, innerException)
        {
        }
    }

    // InternalServerError Exception extends ApiException
    public class InternalServerErrorException : ApiException
    {
        public InternalServerErrorException(string message, object? detail = default, int code = 0, Exception? innerException = null) : base(500, message, detail, code, innerException)
        {
        }
    }
}
