using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Queries.GetRoom;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.Room;
public class GetRoomQueryHandlerTest
{
    [Fact]
    public async Task AdminSeeHidenInfoStudents()
    {
        string phoneNumber = "123-456-7890";
        var studentModel = new StudentModel(
         Id: 1,
         Email: "student@example.com",
         FirstName: "John",
         LastName: "Doe",
         PasswordHash: "hashed_password_value",
         IsConfirmed: true,
         PhoneNumber: phoneNumber,
         IndexNumber: "S123456",
         ReservationId: 10,
         HasAcceptedReservation: true,
         RoomId: 101,
         IsPhoneNumberHidden: true,
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
        roomRepository.Setup(x => x.GetRoomById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(roomModel);

        GetRoomQuery roomQuery = new(102, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, Roles.Administrator), new(ClaimTypes.NameIdentifier, "99") })));
        GetRoomQueryHandler handler = new(roomRepository.Object, studentRepository.Object);

        var room = await handler.Handle(roomQuery, new CancellationToken());

        Assert.Equal(room.Students.First().PhoneNumber, phoneNumber);
    }
    [Fact]
    public async Task UserNotSeeHidenInfoStudents()
    {
        string phoneNumber = "123-456-7890";
        var studentModelMe = new StudentModel(
         Id: 1,
         Email: "student@example.com",
         FirstName: "John",
         LastName: "Doe",
         PasswordHash: "hashed_password_value",
         IsConfirmed: true,
         PhoneNumber: "123-333-333",
         IndexNumber: "S123456",
         ReservationId: 10,
         HasAcceptedReservation: true,
         RoomId: 101,
         IsPhoneNumberHidden: true,
         IsIndexNumberHidden: false
     );
        var studentModelFriend = new StudentModel(
        Id: 1,
        Email: "student@example.com",
        FirstName: "John",
        LastName: "Doe",
        PasswordHash: "hashed_password_value",
        IsConfirmed: true,
        PhoneNumber: phoneNumber,
        IndexNumber: "S123456",
        ReservationId: 10,
        HasAcceptedReservation: true,
        RoomId: 101,
        IsPhoneNumberHidden: true,
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
        .ReturnsAsync(new List<StudentModel> { studentModelFriend });
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(studentModelMe);


        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.GetRoomById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(roomModel);

        GetRoomQuery roomQuery = new(101, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, Roles.Student), new(ClaimTypes.NameIdentifier, "1") })));
        GetRoomQueryHandler handler = new(roomRepository.Object, studentRepository.Object);

        var room = await handler.Handle(roomQuery, new CancellationToken());

        Assert.Null(room.Students.First().PhoneNumber);
    }
    [Fact]
    public async Task StudentInRoomHaveAccess()
    {
        string phoneNumber = "123-456-7890";
        var studentModelMe = new StudentModel(
         Id: 1,
         Email: "student@example.com",
         FirstName: "John",
         LastName: "Doe",
         PasswordHash: "hashed_password_value",
         IsConfirmed: true,
         PhoneNumber: "123-333-333",
         IndexNumber: "S123456",
         ReservationId: 10,
         HasAcceptedReservation: true,
         RoomId: 101,
         IsPhoneNumberHidden: true,
         IsIndexNumberHidden: false
     );
        var studentModelFriend = new StudentModel(
        Id: 1,
        Email: "student@example.com",
        FirstName: "John",
        LastName: "Doe",
        PasswordHash: "hashed_password_value",
        IsConfirmed: true,
        PhoneNumber: phoneNumber,
        IndexNumber: "S123456",
        ReservationId: 10,
        HasAcceptedReservation: true,
        RoomId: 101,
        IsPhoneNumberHidden: true,
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
        .ReturnsAsync(new List<StudentModel> { studentModelFriend });
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(studentModelMe);


        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.GetRoomById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(roomModel);

        GetRoomQuery roomQuery = new(101, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, Roles.Student), new(ClaimTypes.NameIdentifier, "1") })));
        GetRoomQueryHandler handler = new(roomRepository.Object, studentRepository.Object);

        var room = await handler.Handle(roomQuery, new CancellationToken());

        Assert.True(true);
        Assert.NotNull(room);
    }
    [Fact]
    public async Task StudentNotInRoomDontHaveAccess()
    {
        string phoneNumber = "123-456-7890";
        var studentModelMe = new StudentModel(
         Id: 1,
         Email: "student@example.com",
         FirstName: "John",
         LastName: "Doe",
         PasswordHash: "hashed_password_value",
         IsConfirmed: true,
         PhoneNumber: "123-333-333",
         IndexNumber: "S123456",
         ReservationId: 10,
         HasAcceptedReservation: true,
         RoomId: 101,
         IsPhoneNumberHidden: true,
         IsIndexNumberHidden: false
     );
        var studentModelFriend = new StudentModel(
        Id: 2,
        Email: "student@example.com",
        FirstName: "John",
        LastName: "Doe",
        PasswordHash: "hashed_password_value",
        IsConfirmed: true,
        PhoneNumber: phoneNumber,
        IndexNumber: "S123456",
        ReservationId: 10,
        HasAcceptedReservation: true,
        RoomId: 101,
        IsPhoneNumberHidden: true,
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
        .ReturnsAsync(new List<StudentModel> { studentModelFriend });
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(studentModelMe);


        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.GetRoomById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(roomModel);

        GetRoomQuery roomQuery = new(102, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[] { new(ClaimTypes.Role, Roles.Student), new(ClaimTypes.NameIdentifier, "1") })));
        GetRoomQueryHandler handler = new(roomRepository.Object, studentRepository.Object);


        await Assert.ThrowsAsync<UnauthorizedException>(async () =>
        {
            await handler.Handle(roomQuery, new CancellationToken());
        });
    }
}

