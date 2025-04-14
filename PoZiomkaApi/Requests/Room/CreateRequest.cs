using PoZiomkaDomain.Room.Commands.CreateRoom;

namespace PoZiomkaApi.Requests.Room;

public record CreateRequest(int Floor, int Number, int Capacity)
{
    public CreateRoomCommand ToCreateRoomCommand() => new(Floor, Number, Capacity);
}
