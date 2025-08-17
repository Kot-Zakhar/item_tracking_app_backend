using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace ItTrAp.EmailService.Services;

public class EmailSenderService
{
    private readonly ILogger<EmailSenderService> _logger;
    private readonly SmtpSettings _settings;

    public EmailSenderService(ILogger<EmailSenderService> logger, IOptions<GlobalConfig> config)
    {
        _logger = logger;
        _settings = config.Value.Smtp;
    }

    public async Task SendEmailAsync(string name, string email, string subject, string htmlMessage)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            emailMessage.To.Add(new MailboxAddress(name, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_settings.Host, _settings.Port, _settings.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto);
                await client.AuthenticateAsync(_settings.Username, _settings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }

            _logger.LogInformation("Email sent successfully to {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", email);
            throw;
        }
    }
}
