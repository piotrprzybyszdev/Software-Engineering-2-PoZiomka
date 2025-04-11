using MediatR;

namespace PoZiomkaDomain.Room.Commands.RemoveStudent;

public record RemoveStudentCommand(int Id, int StudentId) : IRequest;
