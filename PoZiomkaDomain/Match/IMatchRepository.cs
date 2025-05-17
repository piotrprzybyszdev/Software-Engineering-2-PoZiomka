using PoZiomkaDomain.Match.Dtos;

namespace PoZiomkaDomain.Match;

public interface IMatchRepository
{
    Task<IEnumerable<MatchModel>> GetStudentMatches(int studentId);
    Task UpdateMatch(int matchId, int studentId, MatchStatus status);
    Task<bool> IsMatch(int studentId1, int studentId2);
}
