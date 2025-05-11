using MediatR;

namespace PoZiomkaDomain.Communication.Commands.SendCommunication;

public record SendCommunicationCommand(IEnumerable<int> StudentIds, string Description) : IRequest;