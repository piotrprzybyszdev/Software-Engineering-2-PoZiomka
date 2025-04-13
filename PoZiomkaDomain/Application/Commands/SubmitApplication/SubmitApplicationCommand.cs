using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Application.Commands.SubmitApplication;

public record SubmitApplicationCommand(int Id, IFile File, ClaimsPrincipal User) : IRequest;
