namespace e_commerce.Service.Utils.EmailService
{
    public interface IEmailService : IBaseService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
