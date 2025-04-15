namespace PoZiomkaApi.Requests.StudentAnswer;

public record StudentChoosableAnswerCreateRequest(string Name, bool IsHidden);
public record StudentObligatoryAnswerCreateRequest(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden);
public record StudentAnswerCreateRequest(int FormId, IEnumerable<StudentChoosableAnswerCreateRequest> ChoosableAnswers, IEnumerable<StudentObligatoryAnswerCreateRequest> ObligatoryAnswers);
