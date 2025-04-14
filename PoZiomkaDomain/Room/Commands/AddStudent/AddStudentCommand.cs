using MediatR;

namespace PoZiomkaDomain.Room.Commands.AddStudentToRoom;

public record AddStudentCommand(int Id, int StudentId) : IRequest;
