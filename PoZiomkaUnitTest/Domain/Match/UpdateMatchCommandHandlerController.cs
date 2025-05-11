using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Match.Commands.UpdateMatchCommand;
using PoZiomkaDomain.Match.Dtos;
using PoZiomkaDomain.StudentAnswers.Commands.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaUnitTest.Domain.Match;

public class UpdateMatchCommandHandlerController
{
    [Fact]
    public async Task MatchDoesNotExistThrowsException()
    {
        var matchRepository = new Mock<IMatchRepository>();
        matchRepository.Setup(x => x.GetStudentMatches(It.IsAny<int>()))
            .ReturnsAsync(new List<MatchModel>());

        var user = new ClaimsPrincipal(
           new ClaimsIdentity(new Claim[] {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "1") }));

        UpdateMatchCommand command = new UpdateMatchCommand(user, 1, true);
        var handler = new UpdateMatchCommandHandler(matchRepository.Object);


        await Assert.ThrowsAsync<DomainException>(async () =>
        {
            await handler.Handle(command, default);
        });
    }
}
