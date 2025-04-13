using MediatR;

namespace PoZiomkaDomain.Room.Commands.CreateRoom;

public record CreateRoomCommand(int Floor, int Number, int Capacity) : IRequest;
