namespace PoZiomkaDomain.Common;

public interface IEmailContentGenerator
{
    public string ConfirmationEmailSubject { get; }
    public string PasswordResetEmailSubject { get; }

    public Task<string> GenerateEmailConfirmationEmail(string email);
    public Task<string> GeneratePasswordResetEmail(string email);
}
