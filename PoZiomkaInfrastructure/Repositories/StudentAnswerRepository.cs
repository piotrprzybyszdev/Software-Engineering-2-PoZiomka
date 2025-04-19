
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;
using Dapper;

namespace PoZiomkaInfrastructure.Repositories;

public class StudentAnswerRepository(IDbConnection connection) : IStudentAnswerRepository
{
    public Task CreateAnswer(int studentId, int FormId, IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers, IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAnswer(int formId, int studentId, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<StudentAnswerDisplay> GetStudentAnswer(int formId, int studentId, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<StudentAnswerStatus>> GetStudentFormAnswerStatus(int studentId, CancellationToken? cancellationToken)
    {
        var sql = @"SELECT 
    f.Id AS FormId,
    f.Title AS FormTitle,
    CASE 
        WHEN COUNT(*) = COUNT(CASE WHEN s.AnswerId IS NOT NULL THEN 1 END) THEN 'Filled'
        WHEN COUNT(CASE WHEN s.AnswerId IS NOT NULL THEN 1 END) > 0 THEN 'InProgress'
        ELSE 'NotFilled'
    END AS FormStatusString
FROM StudentAnswer sa
JOIN Forms f ON sa.FormId = f.Id
LEFT JOIN ObligatoryPreferences o ON f.Id = o.FormId
LEFT JOIN StudentAnswerObligatory s ON o.Id = s.ObligatoryPrefernceId
WHERE sa.StudentId = @studentId
GROUP BY f.Id, f.Title;
";

        try
        {
            var studentAnswerStatus = await connection.QueryAsync<StudentAnswerStatus>(
                               new CommandDefinition(sql, new { studentId }, cancellationToken: cancellationToken ?? default));
            return studentAnswerStatus ?? throw new IdNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public Task UpdateAnswer(int studentId, int FormId, IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers, IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers, CancellationToken? cancellationToken)
    {
        throw new NotImplementedException();
    }
}
