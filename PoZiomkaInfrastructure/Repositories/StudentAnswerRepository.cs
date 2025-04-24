
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
using Microsoft.SqlServer.Server;
using System.Transactions;
using System.Runtime.Serialization;
using TransactionException = PoZiomkaInfrastructure.Exceptions.TransactionException;

namespace PoZiomkaInfrastructure.Repositories;

public class StudentAnswerRepository(IDbConnection connection) : IStudentAnswerRepository
{
    public async Task CreateAnswer(int studentId, int FormId, IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers, IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers, CancellationToken? cancellationToken)
    {
        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }
        using var transaction = connection.BeginTransaction();
        try
        {
            // Insert into StudentAnswer and get answerId
            var insertAnswerSql = @"
            INSERT INTO StudentAnswer (StudentId, FormId)
            VALUES (@StudentId, @FormId);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            var answerId = await connection.ExecuteScalarAsync<int>(
            insertAnswerSql,
                new { StudentId = studentId, FormId = FormId }, transaction);

            // Insert into StudentAnswerChoosable
            var insertChoosableSql = @"
            INSERT INTO StudentAnswerChoosable (AnswerId, Name, IsHidden)
            VALUES (@AnswerId, @Name, @IsHidden);";

            foreach (var (name, isHidden) in ChoosableAnswers)
            {
                await connection.ExecuteAsync(
                    insertChoosableSql,
                    new { AnswerId = answerId, Name = name, IsHidden = isHidden }, transaction);
            }

            // Insert into StudentAnswerObligatory
            var insertObligatorySql = @"
            INSERT INTO StudentAnswerObligatory (AnswerId, ObligatoryPrefernceId, ObligatoryPreferenceOptionId, IsHidden)
            VALUES (@AnswerId, @ObligatoryPreferenceId, @ObligatoryPreferenceOptionId, @IsHidden);";

            foreach (var (prefId, optionId, isHidden) in ObligatoryAnswers)
            {
                await connection.ExecuteAsync(
                    insertObligatorySql,
                    new
                    {
                        AnswerId = answerId,
                        ObligatoryPreferenceId = prefId,
                        ObligatoryPreferenceOptionId = optionId,
                        IsHidden = isHidden
                    }, transaction);
            }

            transaction.Commit();
        }
        catch (SqlException exception)
        {
            transaction.Rollback();
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task DeleteAnswer(int formId, int studentId, CancellationToken? cancellationToken)
    {
        var sql = @"DELETE FROM StudentAnswer WHERE FormId = @formId AND StudentId = @studentId";
        int affectedRows;
        try
        {
            affectedRows = await connection.ExecuteAsync(sql, new { formId, studentId });
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
        if (affectedRows == 0)
        {
            throw new UserDidNotAnswerItException("User did not answer this form");
        }
    }
    public async Task DeleteAnswer(int answerId, CancellationToken? cancellationToken)
    {
        var sql = @"DELETE FROM StudentAnswer WHERE Id = @answerId";
        int affectedRows;
        try
        {
            affectedRows = await connection.ExecuteAsync(sql, new { answerId });
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
        if (affectedRows == 0)
        {
            throw new UserDidNotAnswerItException("User did not answer this form");
        }
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
        var sql3 = @"SELECT Id, Name, IsHidden FROM StudentAnswerChoosable WHERE AnswerId = @answerId ";
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
        List<StudentAnswerChoosableDisplay> choosableAnswers;
        try
        {
            choosableAnswers = (await connection.QueryAsync<StudentAnswerChoosableDisplay>(sql3, new { answerId })).ToList();
        }
        catch(SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
        

        StudentAnswerDisplay display = new StudentAnswerDisplay(answerId, formId, studentId, choosableAnswers, lookup.Values.ToList());
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

    public async Task UpdateAnswer(int studentId, int FormId, IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers, IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers, CancellationToken? cancellationToken)
    {
        try
        {
            await DeleteAnswer(FormId, studentId, cancellationToken);
            await CreateAnswer(studentId, FormId, ChoosableAnswers, ObligatoryAnswers, cancellationToken);
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<IEnumerable<StudentAnswerModel>> GetStudentAnswerModels(int studentId, CancellationToken? cancellationToken)
    {
        var sql = "SELECT * FROM StudentAnswer WHERE StudentId = @studentId";
        try
        {
            var studentAnswerModels = await connection.QueryAsync<StudentAnswerModel>(
                               new CommandDefinition(sql, new { studentId }, cancellationToken: cancellationToken ?? default));
            return studentAnswerModels ?? throw new IdNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
