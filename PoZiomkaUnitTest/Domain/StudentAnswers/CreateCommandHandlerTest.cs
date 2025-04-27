using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Exceptions;
using PoZiomkaDomain.StudentAnswers;
using System.Security.Claims;
using PoZiomkaDomain.StudentAnswers.Commands.Create;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaUnitTest.Domain.StudentAnswers;

public class CreateCommandHandlerTest
{
    [Fact]
    public async Task CanNotFillFormThrowExceptionTest()
    {
        var user = new ClaimsPrincipal(
             new ClaimsIdentity(new Claim[] {
            new(ClaimTypes.Role, Roles.Student),
            new(ClaimTypes.NameIdentifier, "2") }));

        Mock<IStudentAnswerRepository> studentAnswerRepository = new();
        Mock<IStudentRepository> studentRepository = new();
        Mock<IFormRepository> formRepository = new();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));
        studentRepository.Setup(x => x.GetStudentByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));
        formRepository.Setup(x => x.GetForms(It.IsAny<CancellationToken>())).ReturnsAsync([new FormModel(1, "Test")]);

        var command = new CreateCommand(user, 1, [], []);
        var handler = new CreateCommandHandler(studentRepository.Object, formRepository.Object, studentAnswerRepository.Object);
        await Assert.ThrowsAsync<UserCanNotFillFormException>(async () => await handler.Handle(command, new CancellationToken()));
    }
}
