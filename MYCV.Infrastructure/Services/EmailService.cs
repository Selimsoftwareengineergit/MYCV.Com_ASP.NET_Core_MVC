using Microsoft.Extensions.Configuration;
using MYCV.Application.Interfaces;
using MYCV.Infrastructure.Services;
using System.Net.Mail;
using System.Net;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly EmailTemplateService _templateService;

    public EmailService(
        IConfiguration configuration,
        EmailTemplateService templateService)
    {
        _configuration = configuration;
        _templateService = templateService;
    }

    public async Task SendVerificationEmailAsync(
        string toEmail,
        string fullName,
        string verificationCode)
    {
        var body = await _templateService
            .LoadTemplateAsync("EmailVerification.html");

        body = body
            .Replace("{{FullName}}", fullName)
            .Replace("{{VerificationCode}}", verificationCode);

        using var smtpClient = new SmtpClient("smtp.gmail.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(
                _configuration["Email:Username"],
                _configuration["Email:Password"]),
            Timeout = 20000
        };

        var message = new MailMessage
        {
            From = new MailAddress(_configuration["Email:From"]),
            Subject = "Verify your email",
            Body = body,
            IsBodyHtml = true
        };

        message.To.Add(toEmail);

        await smtpClient.SendMailAsync(message);
    }
}
