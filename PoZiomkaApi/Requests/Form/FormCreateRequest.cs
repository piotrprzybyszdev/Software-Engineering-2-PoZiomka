using PoZiomkaDomain.Form.Commands;

namespace PoZiomkaApi.Requests.Form;

public record ObligatoryPreferenceCreate(string Name, IEnumerable<string> Options);
public record FormCreateRequest(string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences)
{
    public CreateFormCommand ToCreateFormCommand() =>
        new(Title, ObligatoryPreferences.Select(op => new PoZiomkaDomain.Form.Dtos.ObligatoryPreferenceCreate(op.Name, op.Options)));
}
