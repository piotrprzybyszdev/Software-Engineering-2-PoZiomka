using PoZiomkaDomain.Communication.Dtos;

namespace PoZiomkaDomain.Communication;

public interface ICommunicationRepository
{
    public Task CreateCommunications(IEnumerable<int> studentId, string description, CancellationToken? cancellationToken);
    public Task<IEnumerable<CommunicationDisplay>> GetStudentCommunications(int studentId, CancellationToken? cancellationToken);
    public Task DeleteCommunication(int id, CancellationToken? cancellationToken);
    public Task<int> GetStudentIdByCommunicationId(int id, CancellationToken? cancellationToken);
}
