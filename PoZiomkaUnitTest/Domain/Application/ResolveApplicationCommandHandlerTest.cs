using Moq;
using PoZiomkaDomain.Application;
using PoZiomkaDomain.Application.Commands.ResolveApplication;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Application.Exceptions;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaUnitTest.Domain.Application;

public class ResolveApplicationCommandHandlerTest
{
    [Fact]
    public async Task OnApplicationIdNotFound_ThrowsException()
    {
        var command = new ResolveApplicationCommand(1, ApplicationStatus.Accepted);

        var applicationRepository = new Mock<IApplicationRepository>();

        applicationRepository.Setup(x => x.Update(command.Id, command.Status)).ThrowsAsync(new IdNotFoundException());

        var handler = new ResolveApplicationCommandHandler(applicationRepository.Object);

        await Assert.ThrowsAsync<ApplicationNotFoundException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task ResolvesApplication()
    {
        var command = new ResolveApplicationCommand(1, ApplicationStatus.Accepted);

        var applicationRepository = new Mock<IApplicationRepository>();

        var handler = new ResolveApplicationCommandHandler(applicationRepository.Object);

        await handler.Handle(command, default);

        applicationRepository.Verify(x => x.Update(command.Id, command.Status), Times.Once);
    }
}
