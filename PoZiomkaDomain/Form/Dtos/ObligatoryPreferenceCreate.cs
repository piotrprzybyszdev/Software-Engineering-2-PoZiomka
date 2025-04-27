namespace PoZiomkaDomain.Form.Dtos;

public record ObligatoryPreferenceCreate(string Name, IEnumerable<string> Options);
