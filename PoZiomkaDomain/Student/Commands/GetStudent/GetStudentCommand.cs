using MediatR;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.GetStudent;

public record GetStudentCommand(int Id, ClaimsPrincipal User) : IRequest<StudentDisplay>;

