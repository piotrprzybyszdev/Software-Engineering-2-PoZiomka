﻿using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Match.Dtos;

namespace PoZiomkaDomain.Match.Commands.UpdateMatchCommand;

public class UpdateMatchCommandHandler(IMatchRepository matchRepository) : IRequestHandler<UpdateMatchCommand>
{
    public async Task Handle(UpdateMatchCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");
        MatchStatus status = request.isAccepted ? MatchStatus.Accepted : MatchStatus.Rejected;

        var match = await matchRepository.GetStudentMatches(studentId);
        if (match.All(x => x.Id != request.Id))
            throw new DomainException("Match not found or it is not this studentId match");

        await matchRepository.UpdateMatch(request.Id, studentId, status);
    }
}
