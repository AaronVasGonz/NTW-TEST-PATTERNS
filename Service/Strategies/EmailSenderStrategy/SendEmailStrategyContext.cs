using Service.Services;
using Strategies.Authentication;
using Service.Strategies.EmailSenderStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategies.EmailSenderStrategy;

public interface ISendEmailStrategyContext
{
    Task<bool> SendEmailAsync(string email, string subject, string message, string? token);
    void SetEmailSenderStrategy(IEmailSenderStrategy emailSenderStrategy);
}

public class SendEmailStrategyContext : ISendEmailStrategyContext
{
    private IEmailSenderStrategy _emailSenderStrategy;
    private readonly IEmailSenderService _emailSenderService;

    public SendEmailStrategyContext(IEmailSenderStrategy emailSenderStrategy)
    {
        _emailSenderService = new EmailSenderService();
        _emailSenderStrategy = emailSenderStrategy ?? new SendDefaultEmailStrategy(_emailSenderService);
    }

    public void SetEmailSenderStrategy(IEmailSenderStrategy emailSenderStrategy)
    {
        _emailSenderStrategy = emailSenderStrategy ?? throw new ArgumentNullException(nameof(emailSenderStrategy));
    }

    public async Task<bool> SendEmailAsync(string email, string subject, string message, string? token)
    {
        return await _emailSenderStrategy.SendEmailAsync(email, subject, message, token);
    }
}
