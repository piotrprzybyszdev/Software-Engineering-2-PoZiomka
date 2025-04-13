namespace PoZiomkaDomain.Common.Interface;

public interface IEmailService
{
    public Task SendEmailConfirmationEmail(string receiver);
    public Task SendPasswordResetEmail(string receiver);
}
