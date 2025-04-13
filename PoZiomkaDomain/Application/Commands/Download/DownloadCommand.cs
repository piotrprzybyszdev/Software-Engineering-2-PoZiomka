using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Application.Commands.Download;

public record DownloadCommand(int Id, ClaimsPrincipal User) : IRequest<IFile>;