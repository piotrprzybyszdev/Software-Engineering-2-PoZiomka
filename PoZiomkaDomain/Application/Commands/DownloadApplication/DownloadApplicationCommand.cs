using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Application.Commands.DownloadApplication;

public record DownloadApplicationCommand(int Id, ClaimsPrincipal User) : IRequest<IFile>;