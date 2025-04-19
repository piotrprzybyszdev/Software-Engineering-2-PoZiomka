using PoZiomkaDomain.StudentAnswers.Commands.Update;
using System.Security.Claims;

namespace PoZiomkaApi.Requests.StudentAnswer;

public record StudentAnswerUpdateRequest(int FormId, IEnumerable<StudentChoosableAnswerCreateRequest> ChoosableAnswers, IEnumerable<StudentObligatoryAnswerCreateRequest> ObligatoryAnswers)
{
    public UpdateCommand ToUpdateCommand(ClaimsPrincipal user)
    {
        return new UpdateCommand(user, FormId,
                    ChoosableAnswers.Select(x => (x.Name, x.IsHidden)),
                    ObligatoryAnswers.Select(x => (x.ObligatoryPreferenceId, x.ObligatoryPreferenceOptionId, x.IsHidden)));
    }
}
