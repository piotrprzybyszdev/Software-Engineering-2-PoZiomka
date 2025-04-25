namespace PoZiomkaDomain.Form.Dtos;

public record FormCreate(string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences);
