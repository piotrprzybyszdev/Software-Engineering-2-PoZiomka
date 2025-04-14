using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Common.Interface;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.LoginStudent;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student.Exceptions;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.Student;

public class LoginStudentCommandHandlerTest
{
    [Fact]
    public async Task ReturnsClaims()
    {
        var email = "test@gmail.com";
        var password = "password";
        var hash = "hash";

        var studentModel = new StudentModel
        (
            Id: 1,
            Email: email,
            FirstName: "John",
            LastName: "Doe",
            PasswordHash: hash,
            IsConfirmed: true,
            PhoneNumber: "123456789",
            IndexNumber: "S123456",
            ReservationId: null,
            HasAcceptedReservation: false,
            RoomId: null,
            IsPhoneNumberHidden: false,
            IsIndexNumberHidden: false
        );

        var command = new LoginStudentCommand(email, password);

        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(x => x.VerifyHash(password, hash)).Returns(true);

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentByEmail(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);

        var handler = new LoginStudentCommandHandler(passwordService.Object, studentRepository.Object);
        var result = await handler.Handle(command, default);

        Assert.Contains(result, x => x.Type == ClaimTypes.NameIdentifier && x.Value == studentModel.Id.ToString());
        Assert.Contains(result, x => x.Type == ClaimTypes.Role && x.Value == Roles.Student);
    }

    [Fact]
    public async Task ThrowsUserNotFoundException()
    {
        var email = "test@gmail.com";
        var password = "password";

        var command = new LoginStudentCommand(email, password);

        var passwordService = new Mock<IPasswordService>();

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentByEmail(email, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new EmailNotFoundException());

        var handler = new LoginStudentCommandHandler(passwordService.Object, studentRepository.Object);

        await Assert.ThrowsAsync<StudentNotFoundException>(
            () => handler.Handle(command, default)
        );
    }


    [Fact]
    public async Task OnEmailNotConfirmed_ThrowsEmailNotConfirmedException()
    {
        var email = "test@gmail.com";
        var password = "password";
        var hash = "hash";

        var studentModel = new StudentModel
        (
            Id: 1,
            Email: email,
            FirstName: "John",
            LastName: "Doe",
            PasswordHash: hash,
            IsConfirmed: false,
            PhoneNumber: "123456789",
            IndexNumber: "S123456",
            ReservationId: null,
            HasAcceptedReservation: false,
            RoomId: null,
            IsPhoneNumberHidden: false,
            IsIndexNumberHidden: false
        );

        var command = new LoginStudentCommand(email, password);

        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(x => x.VerifyHash(password, hash)).Returns(true);

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentByEmail(studentModel.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);

        var handler = new LoginStudentCommandHandler(passwordService.Object, studentRepository.Object);

        await Assert.ThrowsAsync<EmailNotConfirmedException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task OnNullPasswordHash_ThrowsPasswordNotSetException()
    {
        var email = "test@gmail.com";
        var password = "password";

        var studentModel = new StudentModel
        (
            Id: 1,
            Email: email,
            FirstName: "John",
            LastName: "Doe",
            PasswordHash: null,
            IsConfirmed: true,
            PhoneNumber: "123456789",
            IndexNumber: "S123456",
            ReservationId: null,
            HasAcceptedReservation: false,
            RoomId: null,
            IsPhoneNumberHidden: false,
            IsIndexNumberHidden: false
        );

        var command = new LoginStudentCommand(email, password);

        var passswordService = new Mock<IPasswordService>();
        passswordService.Setup(x => x.ComputeHash(password)).Returns(() => null!);

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentByEmail(studentModel.Email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);

        var handler = new LoginStudentCommandHandler(passswordService.Object, studentRepository.Object);

        await Assert.ThrowsAsync<PasswordNotSetException>(() => handler.Handle(command, default));
    }

    [Fact]
    public async Task OnInvalidPassword_ThrowsInvalidPasswordException()
    {
        var email = "test@gmail.com";
        var password = "password";
        var hash = "hash";

        var studentModel = new StudentModel
        (
            Id: 1,
            Email: email,
            FirstName: "John",
            LastName: "Doe",
            PasswordHash: hash,
            IsConfirmed: true,
            PhoneNumber: "123456789",
            IndexNumber: "S123456",
            ReservationId: null,
            HasAcceptedReservation: false,
            RoomId: null,
            IsPhoneNumberHidden: false,
            IsIndexNumberHidden: false
        );

        var command = new LoginStudentCommand(email, password);

        var passwordService = new Mock<IPasswordService>();
        passwordService.Setup(x => x.VerifyHash(password, hash)).Returns(false);

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentByEmail(email, It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);

        var handler = new LoginStudentCommandHandler(passwordService.Object, studentRepository.Object);

        await Assert.ThrowsAsync<InvalidPasswordException>(() => handler.Handle(command, default));
    }
}
