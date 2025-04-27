
using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.StudentAnswers.Commands.Create;

public record CreateCommand(ClaimsPrincipal User, int FormId,
    IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers,
    IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers) : IRequest;
