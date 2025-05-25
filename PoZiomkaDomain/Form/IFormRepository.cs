using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form;

public interface IFormRepository
{
    Task<IEnumerable<FormModel>> GetForms(CancellationToken? cancellationToken);
    Task<FormDisplay> GetFormDisplay(int id, CancellationToken? cancellationToken);
    Task CreateForm(FormCreate form, CancellationToken? cancellationToken);
    Task UpdateForm(FormUpdate form, CancellationToken? cancellationToken);
    Task DeleteForm(int id, CancellationToken? cancellationToken);
}
