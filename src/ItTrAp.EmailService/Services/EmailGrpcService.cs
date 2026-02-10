using Grpc.Core;
using ItTrAp.EmailService.Email;

namespace ItTrAp.EmailService.Services;

public class EmailGrpcService : EmailSender.EmailSenderBase
{
    private readonly ILogger<EmailGrpcService> _logger;
    private readonly EmailSenderService _emailSenderService;

    public EmailGrpcService(ILogger<EmailGrpcService> logger, EmailSenderService emailSenderService)
    {
        _logger = logger;
        _emailSenderService = emailSenderService;
    }

    public override async Task<EmailResponse> SendEmail(EmailRequest request, ServerCallContext context)
    {
        try {
            await _emailSenderService.SendEmailAsync(request.ToName, request.ToEmail, request.Subject, request.Body);
            return new EmailResponse { Success = true };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", request.ToEmail);
            return new EmailResponse { Success = false, Message = ex.Message };
        }
    }
}