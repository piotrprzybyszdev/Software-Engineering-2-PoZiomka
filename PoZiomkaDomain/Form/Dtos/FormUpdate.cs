namespace PoZiomkaDomain.Form.Dtos;

public record FormUpdate(int Id, string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences);
