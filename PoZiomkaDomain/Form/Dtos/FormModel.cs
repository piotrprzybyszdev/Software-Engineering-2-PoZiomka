namespace PoZiomkaDomain.Form.Dtos;

public record ObligatoryPreferenceOptionModel(int Id, int PreferenceId, string Name);
public record ObligatoryPreferenceModel(int Id, int FormId, string Name);
public record FormModel(int Id, string Title);
