using Grpc.Net.Client;
using ItTrAp.IdentityService.Email;
using ItTrAp.IdentityService.Infrastructure.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace ItTrAp.IdentityService.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly string _emailServerAddress;

    public EmailService(ILogger<EmailService> logger, IOptions<GlobalConfig> globalConfig)
    {
        _logger = logger;
        _emailServerAddress = globalConfig.Value.EmailServerAddress;
    }

    public async Task SendEmailAsync(string name, string email, string subject, string htmlMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            using var channel = GrpcChannel.ForAddress(_emailServerAddress);
            var client = new EmailSender.EmailSenderClient(channel);

            var response = await client.SendEmailAsync(new EmailRequest
            {
                ToName = name,
                ToEmail = email,
                Subject = subject,
                Body = htmlMessage
            }, cancellationToken: cancellationToken);

            if (!response.Success)
            {
                throw new Exception(response.Message);
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