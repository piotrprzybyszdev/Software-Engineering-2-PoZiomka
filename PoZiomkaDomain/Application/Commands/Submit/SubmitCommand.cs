using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Application.Commands.Submit;

public record SubmitCommand(int Id, IFile File, ClaimsPrincipal User) : IRequest;
