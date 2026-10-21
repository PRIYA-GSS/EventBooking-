using Microsoft.Extensions.Configuration;
using EventHub.Application.Common.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(
        string to,
        string subject,
        string htmlContent)
    {
        var apiKey = _configuration["SendGrid:ApiKey"];
        var fromEmail = _configuration["SendGrid:FromEmail"];
        var fromName = _configuration["SendGrid:FromName"];

        var client = new SendGridClient(apiKey);

        var from = new EmailAddress(fromEmail, fromName);
        var recipient = new EmailAddress(to);

        var message = MailHelper.CreateSingleEmail(
            from,
            recipient,
            subject,
            null,
            htmlContent);

        await client.SendEmailAsync(message);
    }
}