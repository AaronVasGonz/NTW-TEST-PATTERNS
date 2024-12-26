using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategies.EmailSenderStrategy;

 public interface IEmailSenderStrategy
{
    Task<bool> SendEmailAsync(string email, string subject, string message, string? token);
}

