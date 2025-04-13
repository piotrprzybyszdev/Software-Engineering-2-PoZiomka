using Azure.Core;
using Moq;
using PoZiomkaDomain.Application;
using PoZiomkaDomain.Application.Commands.DownloadApplication;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Application.Exceptions;
using PoZiomkaDomain.Common.Exceptions;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.Application;

public class DownloadApplicationCommandHandlerTest
{
    [Fact]
    public async Task OnNullApplication_ThrowsException()
    {
        var command = new DownloadApplicationCommand(1, new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));

        var applicationRepository = new Mock<IApplicationRepository>();
        applicationRepository.Setup(x => x.Get(1))
            .ReturnsAsync((ApplicationModel)null);

        var fileStorage = new Mock<IFileStorage>();

        var file = new Mock<IFile>();
        var stream = new Mock<Stream>();

        var handler = new DownloadApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        var exception = await Assert.ThrowsAsync<ApplicationNotFoundException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task OnDifferentApplicationStudentId_ThrowsException()
    {
        var command = new DownloadApplicationCommand(1, new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));
        var application = new ApplicationModel(1, 2, 0, Guid.NewGuid(), ApplicationStatus.Accepted);

        var applicationRepository = new Mock<IApplicationRepository>();
        applicationRepository.Setup(x => x.Get(1))
            .ReturnsAsync(application);

        var fileStorage = new Mock<IFileStorage>();

        var file = new Mock<IFile>();
        var stream = new Mock<Stream>();

        var handler = new DownloadApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        var exception = await Assert.ThrowsAsync<ApplicationOwnershipException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task OnNoUserId_ThrowsException()
    {
        var command = new DownloadApplicationCommand(1, new ClaimsPrincipal(new ClaimsIdentity()));
        var application = new ApplicationModel(1, 1, 0, Guid.NewGuid(), ApplicationStatus.Accepted);

        var applicationRepository = new Mock<IApplicationRepository>();
        applicationRepository.Setup(x => x.Get(1))
            .ReturnsAsync(application);

        var fileStorage = new Mock<IFileStorage>();

        var file = new Mock<IFile>();
        var stream = new Mock<Stream>();

        var handler = new DownloadApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        var exception = await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, default));
        Assert.Equal("Id of the user isn't known", exception.Message);
    }

    [Fact]
    public async Task OnFileNotFound_ThrowsException()
    {
        var command = new DownloadApplicationCommand(1, new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));
        var guid = Guid.NewGuid();
        var application = new ApplicationModel(1, 1, 0, guid, ApplicationStatus.Accepted);

        var applicationRepository = new Mock<IApplicationRepository>();
        applicationRepository.Setup(x => x.Get(1))
            .ReturnsAsync(application);

        var fileStorage = new Mock<IFileStorage>();

        fileStorage.Setup(x => x.GetFileByGuid(guid)).ThrowsAsync(new PoZiomkaDomain.Application.FileNotFoundException(guid));

        var handler = new DownloadApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        var exception = await Assert.ThrowsAsync<DomainException>(() => handler.Handle(command, default));
        Assert.Equal($"Application with id `{1}` exists but the corresponding file with id `{guid}` doesnt", exception.Message);
    }

    [Fact]
    public async Task DownloadsFile()
    {

        var command = new DownloadApplicationCommand(1, new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));
        var guid = Guid.NewGuid();
        var application = new ApplicationModel(1, 1, 0, guid, ApplicationStatus.Accepted);

        var applicationRepository = new Mock<IApplicationRepository>();
        applicationRepository.Setup(x => x.Get(1))
            .ReturnsAsync(application);

        var fileStorage = new Mock<IFileStorage>();

        var file = new Mock<IFile>();
        var stream = new Mock<Stream>();
        file.SetupGet(x => x.Stream).Returns(stream.Object);

        fileStorage.Setup(x => x.GetFileByGuid(guid)).ReturnsAsync(file.Object);

        var handler = new DownloadApplicationCommandHandler(applicationRepository.Object, fileStorage.Object);

        var result = await handler.Handle(command, default);

        fileStorage.Verify(x => x.GetFileByGuid(guid), Times.Once);
        
        Assert.Equal(file.Object, result);
    }
}