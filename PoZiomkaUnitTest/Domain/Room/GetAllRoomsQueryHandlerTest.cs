using Moq;
using PoZiomkaDomain.Room.Commands.AddStudent;
using PoZiomkaDomain.Room.Commands.AddStudentToRoom;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Exceptions;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Room.Queries.GetAllRooms;

namespace PoZiomkaUnitTest.Domain.Room;

public class GetAllRoomsQueryHandlerTest
{
    [Fact]
    public async Task EmptyRoomsNotThrowError()
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
        studentRepository.Setup(x => x.GetStudentIdsByRoomId(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<int> {  });


        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.GetAllRooms(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<RoomModel> { roomModel});

        GetAllRoomsQuery query = new ();
        GetAllRoomsQueryHandler handler = new(roomRepository.Object, studentRepository.Object);
        await handler.Handle(query, new CancellationToken());
        Assert.True(true);
    }
}

