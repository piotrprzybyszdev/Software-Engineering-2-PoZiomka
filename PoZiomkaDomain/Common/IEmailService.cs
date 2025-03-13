namespace PoZiomkaDomain.Common;

public interface IEmailService
{
    public Task SendEmail(string receiver, string subject, string body);
}
