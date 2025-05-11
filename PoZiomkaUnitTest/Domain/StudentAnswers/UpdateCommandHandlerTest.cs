using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Commands.Update;
using PoZiomkaDomain.StudentAnswers.Exceptions;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.StudentAnswers;

public class UpdateCommandHandlerTest
{
    [Fact]
    public async Task CanNotFillFormThrowExceptionTest()
    {
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[]{
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "2")
            })
        );

        Mock<IStudentAnswerRepository> studentAnswerRepository = new Mock<IStudentAnswerRepository>();
        Mock<IStudentRepository> studentRepository = new();
        Mock<IFormRepository> formRepository = new();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));
        studentRepository.Setup(x => x.GetStudentByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));
        formRepository.Setup(x => x.GetForms(It.IsAny<CancellationToken>())).ReturnsAsync([new FormModel(1, "Test")]);

        var command = new UpdateCommand(user, 1, [], []);
        var handler = new UpdateCommandHandler(studentRepository.Object, formRepository.Object, studentAnswerRepository.Object);
        await Assert.ThrowsAsync<UserCanNotFillFormException>(async () => await handler.Handle(command, new CancellationToken()));
    }
}
