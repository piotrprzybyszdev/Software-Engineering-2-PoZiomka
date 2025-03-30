using MediatR;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Queries.GetStudent;

public record GetStudentQuery(int Id, ClaimsPrincipal User) : IRequest<StudentDisplay>;

