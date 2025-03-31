namespace PoZiomkaDomain.Common.Interface;

public interface IPasswordService
{
    public string ComputeHash(string password);
    public bool VerifyHash(string password, string hash);
}
