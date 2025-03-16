namespace PoZiomkaDomain.Form.Dtos;

public record ObligatoryPreferenceOptionDisplay(int Id, string Name);
public record ObligatoryPreferenceDisplay(int Id, string Name, IEnumerable<ObligatoryPreferenceOptionDisplay> Options);
public record FormDisplay(int Id, string Title, IEnumerable<ObligatoryPreferenceDisplay> ObligatoryPreferences);
