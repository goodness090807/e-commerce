using Microsoft.Extensions.Logging;

namespace e_commerce.Service.Utils.EmailService
{
    /// <summary>
    /// 測試用Email服務
    /// </summary>
    public class DevEmailService : IEmailService
    {
        private readonly ILogger<DevEmailService> _logger;

        public DevEmailService(ILogger<DevEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            _logger.LogInformation($" Email： {email} \r\n Subject： {subject} \r\n Message: {message}");

            return Task.CompletedTask;
        }
    }
}
