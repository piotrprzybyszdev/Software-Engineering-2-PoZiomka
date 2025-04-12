using MediatR;
using PoZiomkaDomain.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Application.Commands.Resolve;

public record ResolveCommand(int Id, ApplicationStatus Status) : IRequest;