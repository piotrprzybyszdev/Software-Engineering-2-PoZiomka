using MediatR;
using PoZiomkaDomain.Room.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Room.Queries.GetRoom;

public record GetRoomQuery(int Id, ClaimsPrincipal User) : IRequest<RoomStudentDisplay>;
