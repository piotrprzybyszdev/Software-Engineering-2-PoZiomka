namespace PoZiomkaDomain.Form.Dtos;

public record ObligatoryPreferenceOptionDisplay(int Id, string Name);
public record ObligatoryPreferenceDisplay(int Id, string Name, IEnumerable<ObligatoryPreferenceOptionDisplay> Options);
public record FormDisplay(int Id, string Title, IEnumerable<ObligatoryPreferenceDisplay> ObligatoryPreferences);
public class ObligatoryPreferenceDisplayList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public List<ObligatoryPreferenceOptionDisplay> Options { get; set; } = new();
    public ObligatoryPreferenceDisplay ToObligatoryPreferenceDisplay() =>
        new(Id, Name, Options);
}
public class FormDisplayList
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public List<ObligatoryPreferenceDisplayList> ObligatoryPreferences { get; set; } = new();
    public FormDisplay ToFormDisplay() =>
        new(Id, Title, ObligatoryPreferences.Select(op => op.ToObligatoryPreferenceDisplay()));
}

