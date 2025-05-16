using Moq;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Commands.AddStudent;
using PoZiomkaDomain.Room.Commands.AddStudentToRoom;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student.Exceptions;

namespace PoZiomkaUnitTest.Domain.Room;

public class AddStudentHandlerTest
{
    [Fact]
    public async Task StudentInRoomThrowsException()
    {
        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());

        var roomRepository = new Mock<IRoomRepository>();

        AddStudentCommand studentCommand = new AddStudentCommand(1, 1);
        AddStudentCommandHandler handler = new(roomRepository.Object, studentRepository.Object);

        await Assert.ThrowsAsync<StudentNotFoundException>(async () =>
                   await handler.Handle(studentCommand, new CancellationToken()));
    }

    [Fact]
    public async Task StudentNotFoundThrowsException()
    {
        var studentModel = new StudentModel(
            Id: 1,
            Email: "student@example.com",
            FirstName: "John",
            LastName: "Doe",
            PasswordHash: "hashed_password_value",
            IsConfirmed: true,
            PhoneNumber: "123-456-7890",
            IndexNumber: "S123456",
            ReservationId: 10,
            HasAcceptedReservation: true,
            RoomId: 5,
            IsPhoneNumberHidden: false,
            IsIndexNumberHidden: false
        );

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);

        var roomRepository = new Mock<IRoomRepository>();

        AddStudentCommand studentCommand = new AddStudentCommand(1, 1);
        AddStudentCommandHandler handler = new(roomRepository.Object, studentRepository.Object);

        await Assert.ThrowsAsync<StudentAlreadyInRoomException>(async () =>
                   await handler.Handle(studentCommand, new CancellationToken()));
    }

    [Fact]
    public async Task RoomNotFoundThrowsException()
    {
        var studentModel = new StudentModel(
            Id: 1,
            Email: "student@example.com",
            FirstName: "John",
            LastName: "Doe",
            PasswordHash: "hashed_password_value",
            IsConfirmed: true,
            PhoneNumber: "123-456-7890",
            IndexNumber: "S123456",
            ReservationId: 10,
            HasAcceptedReservation: true,
            RoomId: null,
            IsPhoneNumberHidden: false,
            IsIndexNumberHidden: false
        );

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);

        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.GetRoomById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());

        AddStudentCommand studentCommand = new AddStudentCommand(1, 1);
        AddStudentCommandHandler handler = new(roomRepository.Object, studentRepository.Object);

        await Assert.ThrowsAsync<RoomNotFoundException>(async () =>
                   await handler.Handle(studentCommand, new CancellationToken()));
    }

    [Fact]
    public async Task RoomOverflowThrowsException()
    {
        var studentModel = new StudentModel(
           Id: 1,
           Email: "student@example.com",
           FirstName: "John",
           LastName: "Doe",
           PasswordHash: "hashed_password_value",
           IsConfirmed: true,
           PhoneNumber: "123-456-7890",
           IndexNumber: "S123456",
           ReservationId: 10,
           HasAcceptedReservation: true,
           RoomId: null,
           IsPhoneNumberHidden: false,
           IsIndexNumberHidden: false
       );

        var roomModel = new RoomDisplay(
            Id: 101,
            Floor: 2,
            Number: 205,
            Capacity: 3,
            StudentCount: 0,
            ReservationId: 10
        );

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);
        studentRepository.Setup(x => x.GetStudentsByRoomId(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<StudentModel> { studentModel, studentModel, studentModel });


        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.GetRoomById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(roomModel);

        AddStudentCommand studentCommand = new AddStudentCommand(1, 1);
        AddStudentCommandHandler handler = new(roomRepository.Object, studentRepository.Object);

        await Assert.ThrowsAsync<RoomFullException>(async () =>
                   await handler.Handle(studentCommand, new CancellationToken()));
    }
}

