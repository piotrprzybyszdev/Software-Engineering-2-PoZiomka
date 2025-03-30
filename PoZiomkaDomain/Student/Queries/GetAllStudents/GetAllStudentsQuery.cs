using MediatR;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Queries.GetAllStudents;

public record GetAllStudentsQuery() : IRequest<IEnumerable<StudentDisplay>>;