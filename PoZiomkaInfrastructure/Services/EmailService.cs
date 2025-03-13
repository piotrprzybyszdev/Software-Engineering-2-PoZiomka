using PoZiomkaDomain.Common;
using System.Net;
using System.Net.Mail;

namespace PoZiomkaInfrastructure.Services;

public class EmailService(string host, int port, string email, string password, bool secure) : IEmailService
{
    private readonly SmtpClient client = new(host, port)
    {
        EnableSsl = secure,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(email, password)
    };

    public async Task SendEmail(string receiver, string subject, string body)
    {
        var message = new MailMessage(email, receiver, subject, body)
        {
            IsBodyHtml = true
        };

        await client.SendMailAsync(message);
    }
}
