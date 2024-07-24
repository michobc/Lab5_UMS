using System.Net.Mail;
using Microsoft.Extensions.Logging;

namespace LabSession5.Infrastructure.Services;

public class EmailNotificationService
{
    private readonly ILogger<EmailNotificationService> _logger;
    private readonly SmtpClient _smtpClient;

    public EmailNotificationService(ILogger<EmailNotificationService> logger, SmtpClient smtpClient)
    {
        _logger = logger;
        _smtpClient = smtpClient;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var mailMessage = new MailMessage("michel@example.inmind.com", to, subject, body);

        try
        {
            await _smtpClient.SendMailAsync(mailMessage);
            _logger.LogInformation("Email sent to {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {To}", to);
        }
    }
}
