using Moq;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Queries.GetAllRooms;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;

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

        var roomModel = new RoomDisplay(
            Id: 101,
            Floor: 2,
            Number: 205,
            Capacity: 3,
            StudentCount: 0,
            ReservationId: 10
        );

        var studentRepository = new Mock<IStudentRepository>();

        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.GetAllRooms(It.IsAny<CancellationToken>())).ReturnsAsync([roomModel]);

        GetAllRoomsQuery query = new();
        GetAllRoomsQueryHandler handler = new(roomRepository.Object);
        await handler.Handle(query, new CancellationToken());
        Assert.True(true);
    }
}

