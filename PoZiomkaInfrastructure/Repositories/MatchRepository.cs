using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Match.Dtos;
using PoZiomkaInfrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaInfrastructure.Repositories;

public class MatchRepository(IDbConnection connection) : IMatchRepository
{
    public async Task<IEnumerable<MatchModel>> GetStudentMatches(int studentId)
    {
        var sql = @"SELECT * FROM Matches WHERE StudentId1 = @studentId OR StudentId2 = @studentId";

        try
        {
            return await connection.QueryAsync<MatchModel>(sql, new { studentId });
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task UpdateMatch(int matchId, int studentId, MatchStatus status)
    {
        var sql = @"Update Matches SET Status1 = @status WHERE Id = @matchId and StudentId1 = @studentId";
        var sql2 = @"Update Matches SET Status2 = @status WHERE Id = @matchId and StudentId2 = @studentId";

        try
        {
            await connection.ExecuteAsync(sql, new { matchId, status, studentId });
            await connection.ExecuteAsync(sql2, new { matchId, status, studentId });
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<MatchModel> CreateMatch(int studentId1, int studentId2)
    {
        var sql = @"INSERT INTO Matches (StudentId1, StudentId2, Status1, Status2) VALUES (@studentId1, @studentId2, Pending, Pending)";

        try
        {
            var matchId = await connection.ExecuteScalarAsync<int>(sql, new { studentId1, studentId2 });
            return new MatchModel(matchId, studentId1, studentId2, MatchStatus.Pending, MatchStatus.Pending);
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
