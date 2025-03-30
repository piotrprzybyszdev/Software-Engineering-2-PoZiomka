using PoZiomkaDomain.Admin.Dtos;

namespace PoZiomkaDomain.Admin;

public class EmailNotUniqueException : Exception;

public class EmailNotFoundException : Exception;

public interface IAdminRepository
{
    public Task<AdminModel> GetAdminByEmail(string email, CancellationToken? cancellationToken);
}
