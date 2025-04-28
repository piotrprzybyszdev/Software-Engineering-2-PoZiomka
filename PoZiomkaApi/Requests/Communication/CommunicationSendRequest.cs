namespace PoZiomkaApi.Requests.Communication;

public record CommunicationSendRequest(IEnumerable<int> StudentIds, string Description);
