namespace PoZiomkaDomain.StudentAnswers.Dtos;

public record StudentAnswerChoosableModel(int Id, int ChoosablePreferenceId, bool IsHidden);
public record StudentAnswerObligatoryModel(int Id, int ObligatoryPrefernceId, int ObligatoryPreferenceOptionId, bool IsHidden);
public record StudentAnswerModel(int Id, int FormId, int StudentId);
