namespace ItTrAp.IdentityService.Infrastructure.Interfaces.Services;

public interface IEmailService
{
    Task SendEmailAsync(string name, string email, string subject, string htmlMessage, CancellationToken cancellationToken = default);
}