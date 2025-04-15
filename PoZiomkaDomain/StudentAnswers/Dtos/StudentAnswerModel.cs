namespace PoZiomkaDomain.StudentAnswers.Dtos;

public record StudentAnswerChoosableModel(int Id, int AnswerId, string Name, bool IsHidden);
public record StudentAnswerObligatoryModel(int Id, int AnswerId, int ObligatoryPrefernceId, int ObligatoryPreferenceOptionId, bool IsHidden);
public record StudentAnswerModel(int Id, int FormId, int StudentId);
