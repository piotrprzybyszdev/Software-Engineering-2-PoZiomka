namespace PoZiomkaApi.Requests.Form;

public record ObligatoryPreferenceCreate(string Name, IEnumerable<string> Options);
public record FormCreateRequest(string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences);
