using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Match.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Match.Commands.UpdateMatchCommand;

public class UpdateMatchCommandHandler(IMatchRepository matchRepository) : IRequestHandler<UpdateMatchCommand>
{
    public async Task Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");
        MatchStatus status = request.isAccepted ? MatchStatus.Accepted : MatchStatus.Rejected;
        await matchRepository.UpdateMatch(request.Id, studentId, status);
    }
}
