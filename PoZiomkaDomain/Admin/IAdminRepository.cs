using PoZiomkaDomain.Admin.Dtos;

namespace PoZiomkaDomain.Admin;

public class EmailNotFoundException : Exception;
public class IdNotFoundException : Exception;

public interface IAdminRepository
{
    public Task<AdminModel> GetAdminByEmail(string email, CancellationToken? cancellationToken);

    public Task<AdminModel> GetAdminById(int id, CancellationToken? cancellationToken);
}
