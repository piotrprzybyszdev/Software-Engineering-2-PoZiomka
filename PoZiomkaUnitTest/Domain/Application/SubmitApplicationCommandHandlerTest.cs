using Moq;
using PoZiomkaDomain.Application;
using PoZiomkaDomain.Application.Commands.SubmitApplication;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Application.Exceptions;
using PoZiomkaDomain.Common.Exceptions;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.Application;

public class SubmitApplicationCommandHandlerTest
{
    [Fact]
    public async Task OnFileTooLarge_ThrowsException()
    {
        var file = new Mock<IFile>();
        var stream = new Mock<Stream>();
        stream.SetupGet(x => x.Length).Returns(100001);
        file.SetupGet(x => x.Stream).Returns(stream.Object);

        var command = new SubmitApplicationCommand(1, file.Object, new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));

        var fileStorage = new Mock<IFileStorage>();
        fileStorage.Setup(x => x.UploadFile(It.IsAny<Guid>(), file.Object)).ThrowsAsync(new FileTooLargeException((int)file.Object.Stream.Length, 0));

        var applicationRepository = new Mock<IApplicationRepository>();

        var handler = new SubmitApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        await Assert.ThrowsAsync<InvalidFileException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task OnInvalidUserId_ThrowsException()
    {
        var file = new Mock<IFile>();
        var stream = new Mock<Stream>();
        stream.SetupGet(x => x.Length).Returns(100001);
        file.SetupGet(x => x.Stream).Returns(stream.Object);

        var command = new SubmitApplicationCommand(1, file.Object, new ClaimsPrincipal(new ClaimsIdentity()));

        var fileStorage = new Mock<IFileStorage>();

        var applicationRepository = new Mock<IApplicationRepository>();

        var handler = new SubmitApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        var exception = await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, default));
        Assert.Equal("Id of the user isn't known", exception.Message);
    }

    [Fact]
    public async Task ApplicationTypeNotFound_ThrowsException()
    {
        var file = new Mock<IFile>();
        var stream = new Mock<Stream>();
        stream.SetupGet(x => x.Length).Returns(100001);
        file.SetupGet(x => x.Stream).Returns(stream.Object);

        var command = new SubmitApplicationCommand(1, file.Object, new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));

        var fileStorage = new Mock<IFileStorage>();
        var applicationRepository = new Mock<IApplicationRepository>();

        applicationRepository.Setup(x => x.Submit(1, command.Id, It.IsAny<Guid>(), ApplicationStatus.Pending)).ThrowsAsync(new IdNotFoundException());

        var handler = new SubmitApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        await Assert.ThrowsAsync<ApplicationTypeNotFoundException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task SubmitsApplication()
    {
        var file = new Mock<IFile>();
        var stream = new Mock<Stream>();
        stream.SetupGet(x => x.Length).Returns(100001);
        file.SetupGet(x => x.Stream).Returns(stream.Object);

        var command = new SubmitApplicationCommand(1, file.Object, new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));

        var fileStorage = new Mock<IFileStorage>();

        var applicationRepository = new Mock<IApplicationRepository>();

        var handler = new SubmitApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        await handler.Handle(command, default);

        applicationRepository.Verify(x => x.Submit(1, command.Id, It.IsAny<Guid>(), ApplicationStatus.Pending), Times.Once);

        fileStorage.Verify(x => x.UploadFile(It.IsAny<Guid>(), file.Object), Times.Once);
    }
}
