using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.Communication.Commands.DeleteCommunication;

public record DeleteCommunicationCommand(int Id, ClaimsPrincipal User) : IRequest;
