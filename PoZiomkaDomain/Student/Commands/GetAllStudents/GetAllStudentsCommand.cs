using MediatR;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.GetAllStudents;

public record GetAllStudentsCommand() : IRequest<IEnumerable<StudentDisplay>>;