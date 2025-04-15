namespace PoZiomkaApi.Requests.Form;

public record FormUpdateRequest(int Id, string Title, IEnumerable<ObligatoryPreferenceCreate> ObligatoryPreferences);
