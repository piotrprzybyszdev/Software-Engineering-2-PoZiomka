using Moq;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Commands.CreateRoom;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Room.Exceptions;

namespace PoZiomkaUnitTest.Domain.Room;
public class CreateRoomCommandHandlerTest
{
    [Fact]
    public async Task RoomNumberNotUniqueThrowsException()
    {
        var roomRepository = new Mock<IRoomRepository>();
        roomRepository.Setup(x => x.CreateRoom(It.IsAny<RoomCreate>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new RoomNumberNotUniqueException());

        CreateRoomCommand roomCommand = new CreateRoomCommand(1, 1, 1);
        CreateRoomCommandHandler handler = new(roomRepository.Object);

        await Assert.ThrowsAsync<RoomNumberTakenException>(async () =>
                          await handler.Handle(roomCommand, new CancellationToken()));
    }
}

