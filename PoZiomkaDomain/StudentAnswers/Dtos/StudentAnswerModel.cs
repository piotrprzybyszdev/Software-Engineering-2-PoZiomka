namespace PoZiomkaDomain.StudentAnswers.Dtos;

public enum FormStatus
{
    NotFilled, InProgress, Filled
}

public record StudentAnswerChoosableModel(int Id, int AnswerId, string Name, bool IsHidden);
public record StudentAnswerObligatoryModel(int Id, int AnswerId, int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden);
public record StudentAnswerModel(int Id, int FormId, int StudentId, FormStatus Status);
