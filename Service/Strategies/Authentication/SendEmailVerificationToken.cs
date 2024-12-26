using System;
using System.Threading.Tasks;
using Service;
using Service.Services;
using Strategies.EmailSenderStrategy;

namespace Service.Strategies.Authentication
{
    public class SendEmailVerificationToken : IEmailSenderStrategy
    {
        private readonly IEmailSenderService _emailSenderService;

        public SendEmailVerificationToken(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message, string token)
        {
            try
            {
                message = $"Your verification token is: {token}";

                bool result = await _emailSenderService.SendEmailAsync(email, subject, message);

                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}
