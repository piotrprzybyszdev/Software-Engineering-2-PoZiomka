using Moq;
using PoZiomkaDomain.Common.Interface;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.SignupStudent;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaUnitTest.Domain;

public class SignupStudentCommandHandlerTest
{
    [Fact]
    public async Task StoresHash()
    {
        SignupStudentCommand command = new("test@gmail.com", "passwd");
        var hash = "hash";

        var emailService = new Mock<IEmailService>();
        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(m => m.ComputeHash(It.IsAny<string>())).Returns(hash);

        var studentRepository = new Mock<IStudentRepository>();

        SignupStudentCommandHandler handler = new(passwordService.Object, studentRepository.Object, emailService.Object);

        await handler.Handle(command, default);

        studentRepository.Verify(
            m => m.RegisterStudent(
                It.Is<StudentRegister>(s => s.PasswordHash == hash), It.IsAny<CancellationToken?>()
            ), Times.Once
        );

        passwordService.Verify(m => m.ComputeHash(command.Password));
    }

    [Fact]
    public async Task ThrowsEmailTaken()
    {
        SignupStudentCommand command = new("test@gmail.com", "passwd");
        var hash = "hash";

        var emailService = new Mock<IEmailService>();
        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(m => m.ComputeHash(It.IsAny<string>())).Returns(hash);

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(
            m => m.RegisterStudent(
                It.IsAny<StudentRegister>(), It.IsAny<CancellationToken?>())
            ).Throws<EmailNotUniqueException>();

        SignupStudentCommandHandler handler = new(passwordService.Object, studentRepository.Object, emailService.Object);

        await Assert.ThrowsAsync<EmailTakenException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task SendsEmail()
    {
        SignupStudentCommand command = new("test@gmail.com", "passwd");
        var hash = "hash";

        var emailService = new Mock<IEmailService>();
        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(m => m.ComputeHash(It.IsAny<string>())).Returns(hash);

        var studentRepository = new Mock<IStudentRepository>();

        SignupStudentCommandHandler handler = new(passwordService.Object, studentRepository.Object, emailService.Object);

        await handler.Handle(command, default);

        emailService.Verify(
            m => m.SendEmailConfirmationEmail("test@gmail.com"), Times.Once
        );
    }
}
