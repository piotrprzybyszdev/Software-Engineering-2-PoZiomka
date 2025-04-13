using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.UpdateStudent;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.Student;

public class UpdateStudentCommandHandlerTest
{
    private readonly Mock<IStudentRepository> mockStudentRepository;
    private readonly UpdateStudentCommandHandler handler;
    public UpdateStudentCommandHandlerTest()
    {
        mockStudentRepository = new Mock<IStudentRepository>();
        handler = new UpdateStudentCommandHandler(mockStudentRepository.Object);
    }

    [Fact]
    public async Task AdminGetAccess()
    {
        var admin = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] {
                new(ClaimTypes.Role, Roles.Administrator),
                new(ClaimTypes.NameIdentifier, "99") }));
        var command = new UpdateStudentCommand(admin, 1, "John", "Doe", "123456789", "123456", false, false);

        await handler.Handle(command, new CancellationToken());

        // check if does not throw exceptions
        Assert.True(true);
    }
    [Fact]
    public async Task UserItSelfGetAccess()
    {
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "1") }));
        var command = new UpdateStudentCommand(user, 1, "John", "Doe", "123456789", "123456", false, false);

        await handler.Handle(command, new CancellationToken());

        // check if does not throw exceptions
        Assert.True(true);
    }
    [Fact]
    public async Task UserCanNotEditAnotherUser()
    {
        var user = new ClaimsPrincipal(
             new ClaimsIdentity(new Claim[] {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "2") }));
        var command = new UpdateStudentCommand(user, 1, "John", "Doe", "123456789", "123456", false, false);

        // check if does not throw exceptions
        await Assert.ThrowsAsync<UnauthorizedException>(async () =>
            await handler.Handle(command, new CancellationToken()));
    }
    [Fact]
    public async Task ThrowExceptionIfUserIdNotExists()
    {
        mockStudentRepository.Setup(x => x.UpdateStudent(It.IsAny<StudentUpdate>(),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());

        var admin = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] {
                new(ClaimTypes.Role, Roles.Administrator),
                new(ClaimTypes.NameIdentifier, "99") }));
        var command = new UpdateStudentCommand(admin, 1, "John", "Doe", "123456789", "123456", false, false);

        // check if does not throw exceptions
        await Assert.ThrowsAsync<UserNotFoundException>(async () =>
            await handler.Handle(command, new CancellationToken()));
    }
}
