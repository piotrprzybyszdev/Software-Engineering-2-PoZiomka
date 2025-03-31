using PoZiomkaDomain.Common.Interface;
using System.Net;
using System.Net.Mail;

namespace PoZiomkaInfrastructure.Services;

public class EmailService(string host, int port, string email, string password, bool secure, IEmailContentGenerator contentGenerator) : IEmailService
{
    private readonly SmtpClient client = new(host, port)
    {
        EnableSsl = secure,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(email, password)
    };

    public async Task SendEmailConfirmationEmail(string receiver)
    {
        var subject = contentGenerator.ConfirmationEmailSubject;
        var body = await contentGenerator.GenerateEmailConfirmationEmail(receiver);

        await SendEmail(receiver, subject, body);
    }

    public async Task SendPasswordResetEmail(string receiver)
    {
        var subject = contentGenerator.PasswordResetEmailSubject;
        var body = await contentGenerator.GeneratePasswordResetEmail(receiver);

        await SendEmail(receiver, subject, body);
    }

    private async Task SendEmail(string receiver, string subject, string body)
    {
        var message = new MailMessage(email, receiver, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}
