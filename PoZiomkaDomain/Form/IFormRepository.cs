using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form;

public interface IFormRepository
{
    Task<IEnumerable<FormModel>> GetForms(CancellationToken? cancellationToken);
    Task<FormDisplay> GetFormDisplay(int id, CancellationToken? cancellationToken);
    Task CreateForm(string title, CancellationToken? cancellationToken);
    Task CreateFormObligatoryPreferences(int formId, string name, CancellationToken? cancellationToken);
    Task CreateFormObligatoryPreferenceOptions(int preferenceId, string name, CancellationToken? cancellationToken);
    Task UpdateForm(int id, string title, CancellationToken? cancellationToken);
    Task DeleteForm(int id, CancellationToken? cancellationToken);
}

