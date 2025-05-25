using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Commands.Delete;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Exceptions;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.StudentAnswers;

public class DeleteCommandHandlerTest
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

        var command = new DeleteCommand(user, 1);
        var handler = new DeleteCommandHandler(studentRepository.Object, studentAnswerRepository.Object);
        await Assert.ThrowsAsync<UserCanNotFillFormException>(async () => await handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task UserNotAnsweredThrowExcetpitonTest()
    {
        var user = new ClaimsPrincipal(
             new ClaimsIdentity(new Claim[] {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "2")
             })
        );

        IEnumerable<StudentAnswerModel> studentAnswerModels =
        [
            new StudentAnswerModel(1,2,2, FormStatus.NotFilled)
        ];

        Mock<IStudentAnswerRepository> studentAnswerRepository = new();
        Mock<IStudentRepository> studentRepository = new();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));
        studentRepository.Setup(x => x.GetStudentByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));

        studentAnswerRepository.Setup(x => x.DeleteAnswer(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UserDidNotAnswerItException("User did not answer it"));

        var command = new DeleteCommand(user, 1);
        var handler = new DeleteCommandHandler(studentRepository.Object, studentAnswerRepository.Object);
        await Assert.ThrowsAsync<UserCanNotFillFormException>(async () => await handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task IfStudentDoesNotHaveThisAnswerIdOrThisAnswerIdIsSomeoneelsesThrowExceptionTest()
    {
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[]{
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "2") }));

        IEnumerable<StudentAnswerModel> studentAnswerModels =
        [
            new StudentAnswerModel(5,2,2, FormStatus.NotFilled)
        ];

        Mock<IStudentAnswerRepository> studentAnswerRepository = new();
        Mock<IStudentRepository> studentRepository = new();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
           .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));
        studentRepository.Setup(x => x.GetStudentByEmail(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentModel(1, "test@gmail.com", null, null, "hash", false, null, null, null, null, null, false, false));

        var command = new DeleteCommand(user, 1);
        var handler = new DeleteCommandHandler(studentRepository.Object, studentAnswerRepository.Object);
        await Assert.ThrowsAsync<UserCanNotFillFormException>(async () => await handler.Handle(command, new CancellationToken()));
    }
}
