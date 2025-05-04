using PoZiomkaDomain.Match.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Match;

public interface IMatchRepository
{
    Task<IEnumerable<MatchModel>> GetStudentMatches(int studentId);
    Task UpdateMatch(int matchId, int studentId, MatchStatus status);
}
