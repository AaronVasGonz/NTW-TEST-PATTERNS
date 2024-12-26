using Arquitecture;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmailAsync(string email, string subject, string message);
    }

    public class EmailSenderService : IEmailSenderService
    {
        public EmailSenderService()
        {
            EnvConfig.Initialize();
        }

        public async Task<bool> SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                string originEmail = Environment.GetEnvironmentVariable("SMTP_GMAILEMAIL");
                int port = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT"));
                string destinationEmail = email;
                string password = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
                string smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER");

                using (MailMessage mailMessage = new MailMessage(originEmail, destinationEmail, subject, message))
                {
                    mailMessage.IsBodyHtml = false;

                    using (SmtpClient smtpClient = new SmtpClient(smtpServer))
                    {
                        smtpClient.EnableSsl = true;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Host = smtpServer;
                        smtpClient.Port = port;
                        smtpClient.Credentials = new System.Net.NetworkCredential(originEmail, password);

                        await smtpClient.SendMailAsync(mailMessage);
                    }
                }

                return true; // Sending succeeded
            }
            catch (Exception ex)
            {
                // Handle the exception (log the error, retry, etc.)
                return false; // Sending failed
            }
        }
    }
}