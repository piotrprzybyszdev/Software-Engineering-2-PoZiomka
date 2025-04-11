using MediatR;

namespace PoZiomkaDomain.Room.Commands.DeleteRoom;

public record DeleteRoomCommand(int Id) : IRequest;
