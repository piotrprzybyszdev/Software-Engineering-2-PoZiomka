using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Application.Commands.Download;

public record DownloadCommand(int Id, ClaimsPrincipal User) : IRequest<IFile>;