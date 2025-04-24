using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.StudentAnswers.Dtos;

public record StudentAnswerChoosableDisplay(int Id, string Name, bool IsHidden);
public record StudentAnswerObligatoryDisplay(int Id, ObligatoryPreferenceDisplay ObligatoryPreference, int ObligatoryPreferenceOptionId, bool IsHidden);
public class StudentAnswerDisplay
{
    public int Id { get; set; }
    public int FormId { get; set; }
    public int StudentId { get; set; }
    public IEnumerable<StudentAnswerChoosableDisplay> ChoosableAnswers { get; set; }
    public IEnumerable<StudentAnswerObligatoryDisplay> ObligatoryAnswers { get; set; }

    public StudentAnswerDisplay(int id, int formId, int studentId,
        IEnumerable<StudentAnswerChoosableDisplay> choosableAnswers,
        IEnumerable<StudentAnswerObligatoryDisplay> obligatoryAnswers)
    {
        Id = id;
        FormId = formId;
        StudentId = studentId;
        ChoosableAnswers = choosableAnswers;
        ObligatoryAnswers = obligatoryAnswers;
    }

    public void RemoveHiddenAnswers()
    {
        if (ChoosableAnswers != null)
            ChoosableAnswers = ChoosableAnswers.Where(x => !x.IsHidden);
        if (ObligatoryAnswers != null)
            ObligatoryAnswers = ObligatoryAnswers.Where(x => !x.IsHidden);
    }
}

public enum FormStatus
{
    NotFilled, InProgress, Filled
}

public record StudentAnswerStatus(FormModel Form, FormStatus Status)
{
    public StudentAnswerStatus(int FormId, string FormTitle, string FormStatusString)
        : this(new FormModel(FormId, FormTitle), FormStatusString switch
        {
            "Filled" => FormStatus.Filled,
            "InProgress" => FormStatus.InProgress,
            _ => FormStatus.NotFilled
        })
    {
    }
}
