using MediatR;
using PoZiomkaDomain.Application.Dtos;

namespace PoZiomkaDomain.Application.Commands.ResolveApplication;

public record ResolveApplicationCommand(int Id, ApplicationStatus Status) : IRequest;