using PoZiomkaDomain.Form.Dtos;
using System.Text.Json.Serialization;

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

public class StudentAnswerStatus
{
    public int? Id { get; set; }
    public FormModel Form { get; set; }
    public FormStatus Status { get; set; }

    [JsonConstructor]
    public StudentAnswerStatus(int? id, FormModel form, FormStatus status)
    {
        Id = id;
        Form = form;
        Status = status;
    }

    public StudentAnswerStatus(int? id, int formId, string title, FormStatus status)
    {
        Id = id;
        Form = new FormModel(formId, title);
        Status = status;
    }
}
