using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.StudentAnswers.Dtos;

public record StudentAnswerChoosableDisplay(int Id, string Name, bool IsHidden);
public record StudentAnswerObligatoryDisplay(int? Id, ObligatoryPreferenceDisplay ObligatoryPreference, int? ObligatoryPreferenceOptionId, bool IsHidden);
public record StudentAnswerDisplay(int? Id, int FormId, int StudentId, FormStatus Status, IEnumerable<StudentAnswerChoosableDisplay> ChoosableAnswers, IEnumerable<StudentAnswerObligatoryDisplay> ObligatoryAnswers)
{
    public StudentAnswerDisplay HideAnswers()
    {
        return new StudentAnswerDisplay(Id, FormId, StudentId, Status,
            ChoosableAnswers.Where(answer => !answer.IsHidden),
            ObligatoryAnswers.Where(answer => !answer.IsHidden)
        );
    }
}

public record StudentAnswerStatus(int? Id, FormModel Form, FormStatus Status)
{
    public StudentAnswerStatus(int? id, int formId, string title, FormStatus status)
        : this(id, new FormModel(formId, title), status)
    {
    }
}
