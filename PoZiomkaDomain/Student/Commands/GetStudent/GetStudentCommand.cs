using MediatR;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.GetStudent;

public record GetStudentCommand(int Id, bool Hide) : IRequest<StudentDisplay>;
public record GetAllStudentsCommand() : IRequest<IEnumerable<StudentDisplay>>;
