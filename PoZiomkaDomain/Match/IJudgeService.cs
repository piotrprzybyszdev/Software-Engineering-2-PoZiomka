using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaDomain.Match;

public interface IJudgeService
{
    public Task FindMatching(CancellationToken? cancellationToken);
    public Task GenerateReservations(CancellationToken? cancellationToken);
}
