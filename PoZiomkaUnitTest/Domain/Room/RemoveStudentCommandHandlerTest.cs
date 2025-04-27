using Moq;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Room.Commands.RemoveStudent;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student.Exceptions;

namespace PoZiomkaUnitTest.Domain.Room;
public class RemoveStudentCommandHandlerTest
{
    [Fact]
    public async Task StudentNotFoundException()
    {
        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new IdNotFoundException());

        RemoveStudentCommand studentCommand = new RemoveStudentCommand(1, 1);
        RemoveStudentCommandHandler handler = new(studentRepository.Object);

        await Assert.ThrowsAsync<StudentNotFoundException>(async () =>
                   await handler.Handle(studentCommand, new CancellationToken()));
    }
    [Fact]
    public async Task StudentNotAssignedToRoomThrowsException()
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
          RoomId: 101,
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
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);

        RemoveStudentCommand studentCommand = new RemoveStudentCommand(102, 1);
        RemoveStudentCommandHandler handler = new(studentRepository.Object);

        await Assert.ThrowsAsync<StudentNotAssignedToRoomException>(async () =>
                   await handler.Handle(studentCommand, new CancellationToken()));
    }
    [Fact]
    public async Task StudentAssignedToRoomIsOk()
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
              RoomId: 101,
              IsPhoneNumberHidden: false,
              IsIndexNumberHidden: false
          );

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(studentModel);

        RemoveStudentCommand studentCommand = new RemoveStudentCommand(101, 1);
        RemoveStudentCommandHandler handler = new(studentRepository.Object);

        await handler.Handle(studentCommand, new CancellationToken());
        Assert.True(true);
    }
}

