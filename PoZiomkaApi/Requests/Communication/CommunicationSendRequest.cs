using PoZiomkaDomain.Communication.Commands.SendCommunication;

namespace PoZiomkaApi.Requests.Communication;

public record CommunicationSendRequest(IEnumerable<int> StudentIds, string Description)
{
    public SendCommunicationCommand ToSendCommunicationCommand() => new(StudentIds, Description);
};
