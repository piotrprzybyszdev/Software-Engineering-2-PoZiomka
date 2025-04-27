using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Exceptions;
using PoZiomkaDomain.StudentAnswers.Queries.GetStudent;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.StudentAnswers;

public class GetStudentAnswersQueryHandlerTest
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
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));
        studentRepository.Setup(x => x.GetStudentByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));

        var command = new GetStudentAnswersQuery(user);
        var handler = new GetStudentAnswersQueryHandler(studentRepository.Object, studentAnswerRepository.Object);
        await Assert.ThrowsAsync<UserCanNotFillFormException>(async () => await handler.Handle(command, new CancellationToken()));
    }
}
