using Moq;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Common.Interface;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.ConfirmStudent;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain;

public class ConfirmStudentCommandHandlerTest
{
    [Fact]
    public async Task StoresConfirmation()
    {
        var command = new ConfirmStudentCommand("token");

        var studentRepository = new Mock<IStudentRepository>();

        var claim = new ClaimsIdentity([new Claim(ClaimTypes.Email, "test@gmail.com")]);
        var jwtService = new Mock<IJwtService>();
        jwtService.Setup(m => m.ReadToken(It.IsAny<string>())).ReturnsAsync(claim);

        var handler = new ConfirmStudentCommandHandler(studentRepository.Object, jwtService.Object);
        await handler.Handle(command, default);

        studentRepository.Verify(
            m => m.ConfirmStudent(It.IsAny<StudentConfirm>(), It.IsAny<CancellationToken?>()), Times.Once
        );
    }

    [Fact]
    public async Task ThrowsNotATokenException()
    {
        var command = new ConfirmStudentCommand("token");

        var studentRepository = new Mock<IStudentRepository>();

        var jwtService = new Mock<IJwtService>();
        jwtService.Setup(m => m.ReadToken(It.IsAny<string>()))
            .ThrowsAsync(new NotATokenException());

        var handler = new ConfirmStudentCommandHandler(studentRepository.Object, jwtService.Object);

        await Assert.ThrowsAsync<InvalidTokenException>(
            () => handler.Handle(command, default)
        );
    }

    [Fact]
    public async Task ThrowsTokenExpiredException()
    {
        var command = new ConfirmStudentCommand("token");

        var studentRepository = new Mock<IStudentRepository>();

        var jwtService = new Mock<IJwtService>();
        jwtService.Setup(m => m.ReadToken(It.IsAny<string>()))
            .ThrowsAsync(new TokenExpiredException());

        var handler = new ConfirmStudentCommandHandler(studentRepository.Object, jwtService.Object);

        await Assert.ThrowsAsync<InvalidTokenException>(
            () => handler.Handle(command, default)
        );
    }

    [Fact]
    public async Task ThrowsTokenValidationException()
    {
        var command = new ConfirmStudentCommand("token");

        var studentRepository = new Mock<IStudentRepository>();

        var jwtService = new Mock<IJwtService>();
        jwtService.Setup(m => m.ReadToken(It.IsAny<string>()))
            .ThrowsAsync(new TokenValidationException());

        var handler = new ConfirmStudentCommandHandler(studentRepository.Object, jwtService.Object);

        await Assert.ThrowsAsync<InvalidTokenException>(
            () => handler.Handle(command, default)
        );
    }

    [Fact]
    public async Task EmailClaimNotFoundInToken()
    {
        var command = new ConfirmStudentCommand("token");

        var studentRepository = new Mock<IStudentRepository>();

        var emptyClaim = new ClaimsIdentity();
        var jwtService = new Mock<IJwtService>();
        jwtService.Setup(m => m.ReadToken(It.IsAny<string>()))
            .ReturnsAsync(emptyClaim);

        var handler = new ConfirmStudentCommandHandler(studentRepository.Object, jwtService.Object);

        await Assert.ThrowsAsync<InvalidTokenException>(
            () => handler.Handle(command, default)
        );
    }

    [Fact]
    public async Task ThrowsEmailNotFoundException()
    {
        var command = new ConfirmStudentCommand("token");

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(m => m.ConfirmStudent(
                It.IsAny<StudentConfirm>(), It.IsAny<CancellationToken?>()
            )).ThrowsAsync(new EmailNotFoundException());

        var claim = new ClaimsIdentity([new Claim(ClaimTypes.Email, "test@gmail.com")]);
        var jwtService = new Mock<IJwtService>();
        jwtService.Setup(m => m.ReadToken(It.IsAny<string>())).ReturnsAsync(claim);

        var handler = new ConfirmStudentCommandHandler(studentRepository.Object, jwtService.Object);

        await Assert.ThrowsAsync<UserNotFoundException>(
            () => handler.Handle(command, default)
        );
    }
}