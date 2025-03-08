using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.SignupStudent;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaTest.Domain;

public class SignupSutdentCommandHandlerTest
{
    [Fact]
    public async Task StoresHash()
    {
        SignupStudentCommand command = new("test@gmail.com", "passwd");
        var hash = "hash";

        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(m => m.ComputeHash(It.IsAny<string>())).Returns(hash);

        var studentRepository = new Mock<IStudentRepository>();

        SignupStudentCommandHandler handler = new(passwordService.Object, studentRepository.Object);

        await handler.Handle(command, default);

        studentRepository.Verify(
            m => m.CreateStudent(
                It.Is<StudentCreate>(s => s.PasswordHash == hash), It.IsAny<CancellationToken?>()
            ), Times.Once
        );

        passwordService.Verify(m => m.ComputeHash(command.Password));
    }

    [Fact]
    public async Task ThrowsEmailTaken()
    {
        SignupStudentCommand command = new("test@gmail.com", "passwd");
        var hash = "hash";

        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(m => m.ComputeHash(It.IsAny<string>())).Returns(hash);

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(
            m => m.CreateStudent(
                It.IsAny<StudentCreate>(), It.IsAny<CancellationToken?>())
            ).Throws<EmailNotUniqueException>();

        SignupStudentCommandHandler handler = new(passwordService.Object, studentRepository.Object);

        await Assert.ThrowsAsync<EmailTakenException>(() => handler.Handle(command, default));
    }
}
