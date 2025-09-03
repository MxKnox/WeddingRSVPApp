using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using WeddingApp.Identity;
using WeddingApp.Options;

namespace WeddingApp.Mail;

public class EmailService : IEmailSender<WeddingAppUser>
{
    private readonly IOptionsMonitor<EmailOptions> _emailSettings;
    private readonly ILogger<EmailService> _logger;
    
    public EmailService(IOptionsMonitor<EmailOptions> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings;
        _logger = logger;
    }
    
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        switch (_emailSettings.CurrentValue.Provider)
        {
            case EmailProvider.Smtp:
                // Implement SMTP sending
                _logger.LogInformation($"Sending email to {email} via SMTP: {subject}");
                // ... SMTP implementation
                //TODO: SMTP Implementation
                break;
            case EmailProvider.SendGrid:
                // Implement SendGrid sending
                _logger.LogInformation($"Sending email to {email} via SendGrid: {subject}");
            // var apiKey = _configuration["EmailSettings:SendGrid:ApiKey"];
            // var client = new SendGridClient(apiKey);
            // ... implementation
            //TODO: Sendgrid Implementation
                break;
        }
        
        await Task.CompletedTask;
    }

    public async Task SendConfirmationLinkAsync(WeddingAppUser user, string email, string confirmationLink)
    {
        string body = $"""Click <a href="{confirmationLink}">here</a> to confirm your email""";

        await SendEmailAsync(user.Email, "Confirm Email", body);
    }

    public async Task SendPasswordResetLinkAsync(WeddingAppUser user, string email, string resetLink)
    {
        string body = $"""Click <a href="{resetLink}">here</a> to confirm your email""";

        await SendEmailAsync(user.Email, "Reset Password", body);
    }

    public async Task SendPasswordResetCodeAsync(WeddingAppUser user, string email, string resetCode)
    {
        throw new NotImplementedException();
    }
}