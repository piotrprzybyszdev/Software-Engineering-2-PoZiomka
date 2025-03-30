using MediatR;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.GetAllStudent;

public record GetAllStudentsCommand() : IRequest<IEnumerable<StudentDisplay>>;