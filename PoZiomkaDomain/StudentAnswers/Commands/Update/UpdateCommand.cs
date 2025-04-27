using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.StudentAnswers.Commands.Update;
public record UpdateCommand(ClaimsPrincipal User, int formId,
    IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers,
    IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers)
    : IRequest;

