using MediatR;
using PoZiomkaDomain.Communication.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Communication.Queries.GetStudentCommunications;

public record GetStudentCommunicationsQuery(ClaimsPrincipal User) : IRequest<IEnumerable<CommunicationDisplay>>;
