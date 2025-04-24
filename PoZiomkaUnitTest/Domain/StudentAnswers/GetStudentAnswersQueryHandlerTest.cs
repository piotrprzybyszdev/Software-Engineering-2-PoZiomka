using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Commands.Delete;
using PoZiomkaDomain.StudentAnswers.Exceptions;
using PoZiomkaDomain.StudentAnswers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using PoZiomkaDomain.StudentAnswers.Queries.GetStudent;

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

        Mock<IStudentAnswerRepository> studentAnswerRepository = new Mock<IStudentAnswerRepository>();
        Mock<IStudentService> studentService = new Mock<IStudentService>();
        studentService.Setup(x => x.CanFillForm(It.IsAny<int>())).ReturnsAsync(false);

        var command = new GetStudentAnswersQuery(user);
        var handler = new GetStudentAnswersQueryHandler(studentAnswerRepository.Object, studentService.Object);
        await Assert.ThrowsAsync<UserCanNotFillFormException>(async () => await handler.Handle(command, new CancellationToken()));
    }
}
