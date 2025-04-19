using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.StudentAnswers.Commands.Update;
public record UpdateCommand(ClaimsPrincipal User, int formId,
    IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers,
    IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers)
    :IRequest;

