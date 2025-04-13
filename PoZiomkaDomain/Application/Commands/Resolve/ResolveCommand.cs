using MediatR;
using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaDomain.Application.Commands.Resolve;

public record ResolveCommand(int Id, ApplicationStatus Status) : IRequest;