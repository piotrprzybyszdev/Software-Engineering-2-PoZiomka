
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using PoZiomkaDomain.Form.Dtos;
using System.Linq;

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
    public record StudentAnswerObligatoryDto(int StudentAnswerObligatoryId, int ObligatoryPreferenceOptionId, bool IsHidden);
    public record ObligatoryPreferenceDto(int ObligatoryPreferenceId, string ObligatoryPreferenceName);
    public async Task<StudentAnswerDisplay> GetStudentAnswer(int formId, int studentId, CancellationToken? cancellationToken)
    {
        var sql1 = @"SELECT Id From StudentAnswer WHERE StudentId = @studentId And FormId = @formId";
        var sql2 = @"SELECT 
                   s.Id as StudentAnswerObligatoryId, 
                   s.ObligatoryPreferenceOptionId as ObligatoryPreferenceOptionId,
	               s.IsHidden as IsHidden,
       
                   o.Id as ObligatoryPreferenceId,
	               o.Name as ObligatoryPreferenceName,
	   
                   op.Id as Id,
	               op.Name as Name
	   
	
	                FROM StudentAnswerObligatory as s
	                JOIN ObligatoryPreferences as o on s.ObligatoryPrefernceId = o.Id 
	                JOIN ObligatoryPreferenceOptions as op on op.PreferenceId = o.Id 
	                WHERE AnswerId = @answerId ";
        int answerId;
        try
        {
            answerId = await connection.QuerySingleOrDefaultAsync<int>(sql1, new { studentId, formId });
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        var lookup = new Dictionary<int, StudentAnswerObligatoryDisplay>();

        try
        {
            var result = await connection.QueryAsync<
                StudentAnswerObligatoryDto,
                ObligatoryPreferenceDto,
                ObligatoryPreferenceOptionDisplay,
                StudentAnswerObligatoryDisplay
            >(sql2,
            (studentDto, preferenceDto, optionDto) =>
            {
                if (!lookup.TryGetValue(studentDto.StudentAnswerObligatoryId, out var answer))
                {
                    answer = new StudentAnswerObligatoryDisplay(
                        Id: studentDto.StudentAnswerObligatoryId,
                        ObligatoryPreference: new ObligatoryPreferenceDisplay(
                            Id: preferenceDto.ObligatoryPreferenceId,
                            Name: preferenceDto.ObligatoryPreferenceName,
                            Options: new List<ObligatoryPreferenceOptionDisplay>()
                        ),
                        ObligatoryPreferenceOptionId: studentDto.ObligatoryPreferenceOptionId,
                        IsHidden: studentDto.IsHidden
                    );
                    lookup[studentDto.StudentAnswerObligatoryId] = answer;
                }

                var options = (List<ObligatoryPreferenceOptionDisplay>)answer.ObligatoryPreference.Options;
                if (!options.Any(o => o.Id == optionDto.Id))
                {
                    options.Add(optionDto);
                }

                return answer;
            },
            splitOn: "ObligatoryPreferenceId,Id",
            param: new { answerId });
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        StudentAnswerDisplay display = new StudentAnswerDisplay(answerId, formId, studentId, new List<StudentAnswerChoosableDisplay>(), lookup.Values.ToList());
        return display ?? throw new IdNotFoundException();
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
