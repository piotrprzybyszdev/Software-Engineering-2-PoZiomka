using PoZiomkaDomain.Form.Commands.UpdateForm;

namespace PoZiomkaApi.Requests.Form;

public record FormUpdateRequest(int Id, string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences)
{
    public UpdateFormCommand ToUpdateFormCommand() =>
        new(Id, Title, ObligatoryPreferences.Select(op => new PoZiomkaDomain.Form.Dtos.ObligatoryPreferenceCreate(op.Name, op.Options)));
}