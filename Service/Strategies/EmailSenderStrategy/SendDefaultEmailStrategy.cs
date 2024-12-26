using Service.Services;
using Strategies.EmailSenderStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Strategies.EmailSenderStrategy
{
    public class SendDefaultEmailStrategy : IEmailSenderStrategy
    {
        private readonly IEmailSenderService _emailSenderService;

        public SendDefaultEmailStrategy(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message, string? token)
        {
            try
            {
                bool result = await _emailSenderService.SendEmailAsync(email, subject, message);

                return result;  // Simplified: Directly return the result
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}