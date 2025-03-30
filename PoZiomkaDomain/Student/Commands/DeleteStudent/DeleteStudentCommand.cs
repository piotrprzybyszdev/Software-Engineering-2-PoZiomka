using MediatR;

namespace PoZiomkaDomain.Student.Commands.DeleteStudent;

public record DeleteStudentCommand(int Id) : IRequest;