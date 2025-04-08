using PoZiomkaDomain.Admin.Dtos;

namespace PoZiomkaDomain.Admin;

public interface IApplicationRepository
{
    public Task<AdminModel> GetAdminByEmail(string email, CancellationToken? cancellationToken);

    public Task<AdminModel> GetAdminById(int id, CancellationToken? cancellationToken);
}
