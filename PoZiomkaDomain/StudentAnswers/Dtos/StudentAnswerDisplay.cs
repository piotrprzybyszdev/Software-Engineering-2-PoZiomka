using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.StudentAnswers.Dtos;

public record StudentAnswerChoosableDisplay(int Id, string Name, bool IsHidden);
public record StudentAnswerObligatoryDisplay(int Id, ObligatoryPreferenceDisplay ObligatoryPreference, ObligatoryPreferenceOptionDisplay ObligatoryPreferenceOption, bool IsHidden);
public record StudentAnswerDisplay(int Id, int FormId, IEnumerable<StudentAnswerChoosableDisplay> ChoosableAnswers, IEnumerable<StudentAnswerObligatoryDisplay> ObligatoryAnswers);
