using MediatR;
using PoZiomkaDomain.Application.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Application.Queries.GetStudent;
public record GetStudentQuery(ClaimsPrincipal User) : IRequest<IEnumerable<ApplicationDisplay>>;
