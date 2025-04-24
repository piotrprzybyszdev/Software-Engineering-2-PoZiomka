using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Commands.Delete;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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

        Mock<IStudentAnswerRepository> studentAnswerRepository = new Mock<IStudentAnswerRepository>();
        Mock<IStudentService> studentService = new Mock<IStudentService>();
        studentService.Setup(x => x.CanFillForm(It.IsAny<int>())).ReturnsAsync(false);

        var command = new DeleteCommand(user, 1);
        var handler = new DeleteCommandHandler(studentAnswerRepository.Object, studentService.Object);
        await Assert.ThrowsAsync<UserCanNotFillFormException>(async () => await handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task UserNotAnsweredThrowExcetpitonTest()
    {
        var user = new ClaimsPrincipal(
             new ClaimsIdentity(new Claim[] {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "2") }));

        IEnumerable<StudentAnswerModel> studentAnswerModels = new List<StudentAnswerModel>
        {
            new StudentAnswerModel(1,2,2)
        };

        Mock<IStudentAnswerRepository> studentAnswerRepository = new Mock<IStudentAnswerRepository>();
        studentAnswerRepository.Setup(x => x.GetStudentAnswerModels(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(studentAnswerModels);
        studentAnswerRepository.Setup(x=> x.DeleteAnswer(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UserDidNotAnswerItException("User did not answer it"));
        Mock<IStudentService> studentService = new Mock<IStudentService>();
        studentService.Setup(x => x.CanFillForm(It.IsAny<int>())).ReturnsAsync(true);

        var command = new DeleteCommand(user, 1);
        var handler = new DeleteCommandHandler(studentAnswerRepository.Object, studentService.Object);
        await Assert.ThrowsAsync<DomainException>(async () => await handler.Handle(command, new CancellationToken()));
    }

    [Fact]
    public async Task IfStudentDoesNotHaveThisAnswerIdOrThisAnswerIdIsSomeoneelsesThrowExceptionTest()
    {
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(new Claim[] {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "2") }));

        IEnumerable<StudentAnswerModel> studentAnswerModels = new List<StudentAnswerModel>
        {
            new StudentAnswerModel(5,2,2)
        };

        Mock<IStudentAnswerRepository> studentAnswerRepository = new Mock<IStudentAnswerRepository>();
        studentAnswerRepository.Setup(x => x.GetStudentAnswerModels(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(studentAnswerModels);
        Mock<IStudentService> studentService = new Mock<IStudentService>();
        studentService.Setup(x => x.CanFillForm(It.IsAny<int>())).ReturnsAsync(true);

        var command = new DeleteCommand(user, 1);
        var handler = new DeleteCommandHandler(studentAnswerRepository.Object, studentService.Object);
        await Assert.ThrowsAsync<DomainException>(async () => await handler.Handle(command, new CancellationToken()));
    }
}
