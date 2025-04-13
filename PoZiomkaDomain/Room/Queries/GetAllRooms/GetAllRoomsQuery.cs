using MediatR;
using PoZiomkaDomain.Room.Dtos;

namespace PoZiomkaDomain.Room.Queries.GetAllRooms;

public record GetAllRoomsQuery : IRequest<IEnumerable<RoomDisplay>>;
