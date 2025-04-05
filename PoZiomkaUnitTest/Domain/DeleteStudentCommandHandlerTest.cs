using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.DeleteStudent;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain;

public class DeleteStudentCommandHandlerTest
{
    [Fact]
    public async Task DeletesStudent()
    {
        var admin = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] {
                        new(ClaimTypes.Role, Roles.Administrator),
                        new(ClaimTypes.NameIdentifier, "99") }));
        DeleteStudentCommand command = new(1, admin);

        var studentRepository = new Mock<IStudentRepository>();

        var handler = new DeleteStudentCommandHandler(studentRepository.Object);
        await handler.Handle(command, default);

        studentRepository.Verify(x => x.DeleteStudent(It.IsAny<int>(), It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Fact]
    public async Task ThrowExceptionIfUserNotExists()
    {
        var admin = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] {
                        new(ClaimTypes.Role, Roles.Administrator),
                        new(ClaimTypes.NameIdentifier, "99") }));
        DeleteStudentCommand command = new(1, admin);

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.DeleteStudent(It.IsAny<int>(),
            It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());

        DeleteStudentCommandHandler handler = new(studentRepository.Object);

        await Assert.ThrowsAsync<UserNotFoundException>(
            () => handler.Handle(command, default)
        );
    }
}
