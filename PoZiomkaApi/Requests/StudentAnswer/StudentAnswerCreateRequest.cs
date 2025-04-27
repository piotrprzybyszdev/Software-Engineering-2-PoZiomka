using PoZiomkaDomain.StudentAnswers.Commands.Create;
using System.Security.Claims;

namespace PoZiomkaApi.Requests.StudentAnswer;

public record StudentChoosableAnswerCreateRequest(string Name, bool IsHidden);
public record StudentObligatoryAnswerCreateRequest(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden);
public record StudentAnswerCreateRequest(int FormId, IEnumerable<StudentChoosableAnswerCreateRequest> ChoosableAnswers, IEnumerable<StudentObligatoryAnswerCreateRequest> ObligatoryAnswers)
{
    public CreateCommand ToCreateCommand(ClaimsPrincipal user)
    {
        return new CreateCommand(user, FormId,
                ChoosableAnswers.Select(x => (x.Name, x.IsHidden)),
                ObligatoryAnswers.Select(x => (x.ObligatoryPreferenceId, x.ObligatoryPreferenceOptionId, x.IsHidden)));
    }
}
