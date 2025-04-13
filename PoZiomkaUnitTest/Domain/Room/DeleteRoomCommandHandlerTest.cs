using Moq;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Commands.DeleteRoom;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaUnitTest.Domain.Room;
public class DeleteRoomCommandHandlerTest
{
    [Fact]
    public async Task DeleteNotEmptyRoomThrowsException()
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

        var roomModel = new RoomModel(
            Id: 101,
            Floor: 2,
            Number: 205,
            Capacity: 3
        );

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentsByRoomId(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<StudentModel> { studentModel });


        var roomRepository = new Mock<IRoomRepository>();

        DeleteRoomCommand deleteRoom = new DeleteRoomCommand(1);
        DeleteRoomCommandHandler handler = new(roomRepository.Object, studentRepository.Object);

        await Assert.ThrowsAsync<RoomNotEmptyException>(async () =>
                   await handler.Handle(deleteRoom, new CancellationToken()));
    }

    [Fact]
    public async Task RoomNotFoundException()
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

        var roomModel = new RoomModel(
            Id: 101,
            Floor: 2,
            Number: 205,
            Capacity: 3
        );

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentsByRoomId(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<StudentModel> { });


        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.DeleteRoom(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());

        DeleteRoomCommand deleteRoom = new DeleteRoomCommand(1);
        DeleteRoomCommandHandler handler = new(roomRepository.Object, studentRepository.Object);

        await Assert.ThrowsAsync<RoomNotFoundException>(async () =>
                   await handler.Handle(deleteRoom, new CancellationToken()));
    }
}

