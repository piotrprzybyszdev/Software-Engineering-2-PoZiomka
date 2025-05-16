using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class StudentAnswerRepository(IDbConnection connection) : IStudentAnswerRepository
{
    public async Task CreateAnswer(int studentId, int formId, FormStatus status, IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers, IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers, CancellationToken? cancellationToken)
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
            INSERT INTO StudentAnswers (StudentId, FormId, FormStatus)
            VALUES (@StudentId, @FormId, @Status);
            SELECT CAST(SCOPE_IDENTITY() as int);";

            // CHANGE FORM STATUS
            var answerId = await connection.ExecuteScalarAsync<int>(
            insertAnswerSql,
                new { StudentId = studentId, FormId = formId, Status = status }, transaction);

            // Insert into StudentAnswerChoosable
            var insertChoosableSql = @"
            INSERT INTO StudentAnswersChoosable (AnswerId, Name, IsHidden)
            VALUES (@AnswerId, @Name, @IsHidden);";

            foreach (var (name, isHidden) in ChoosableAnswers)
            {
                await connection.ExecuteAsync(
                    insertChoosableSql,
                    new { AnswerId = answerId, Name = name, IsHidden = isHidden }, transaction);
            }

            // Insert into StudentAnswerObligatory
            var insertObligatorySql = @"
            INSERT INTO StudentAnswersObligatory (AnswerId, ObligatoryPreferenceId, ObligatoryPreferenceOptionId, IsHidden)
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
        var sql = @"DELETE FROM StudentAnswers WHERE FormId = @formId AND StudentId = @studentId";
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
        var sql = @"DELETE FROM StudentAnswers WHERE Id = @answerId";
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

    public record StudentAnswersObligatoryFlat(
        int ObligatoryId, int PreferenceId, string PreferenceName, int OptionId, string OptionName,
        int? ChosenId, bool? IsHidden);

    public async Task<StudentAnswerDisplay> GetStudentAnswer(int formId, int studentId, CancellationToken? cancellationToken)
    {
        var sql1 = @"SELECT Id, FormId, StudentId, FormStatus AS Status From StudentAnswers WHERE StudentId = @studentId And FormId = @formId";
        var sql2 = @"
SELECT obligatory.Id as ObligatoryId, preference.Id as PreferenceId, preference.Name as PreferenceName, opt.Id as OptionId, opt.Name as OptionName, obligatory.ObligatoryPreferenceOptionId as chosenId, obligatory.IsHidden FROM Forms form
LEFT JOIN StudentAnswers answer ON answer.FormId = form.Id AND answer.StudentId = @studentId
LEFT JOIN ObligatoryPreferences preference ON form.Id = preference.FormId
LEFT JOIN ObligatoryPreferenceOptions opt ON preference.Id = opt.PreferenceId
LEFT JOIN StudentAnswersObligatory obligatory ON obligatory.AnswerId = answer.Id AND obligatory.ObligatoryPreferenceId = preference.Id
WHERE form.Id = @formId";
        var sql3 = @"SELECT Id, Name, IsHidden FROM StudentAnswersChoosable WHERE AnswerId = @answerId ";

        StudentAnswerModel? answer;
        try
        {
            answer = await connection.QuerySingleOrDefaultAsync<StudentAnswerModel>(sql1, new { formId, studentId });
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        IEnumerable<StudentAnswerObligatoryDisplay> obligatoryAnswers;
        try
        {
            var result = await connection.QueryAsync<StudentAnswersObligatoryFlat>(sql2,
            param: new { formId, studentId });

            obligatoryAnswers = result.GroupBy(row => row.PreferenceId)
                .Select(group => new StudentAnswerObligatoryDisplay(
                    group.First().ObligatoryId, new ObligatoryPreferenceDisplay(
                        group.First().PreferenceId, group.First().PreferenceName,
                        group.Select(grp => new ObligatoryPreferenceOptionDisplay(
                            grp.OptionId, grp.OptionName
                        ))
                    ),
                    group.First().ChosenId, group.First().IsHidden ?? false
                ));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        IEnumerable<StudentAnswerChoosableDisplay> choosableAnswers = [];
        try
        {
            if (answer is not null)
            {
                choosableAnswers = await connection.QueryAsync<StudentAnswerChoosableDisplay>(sql3, new { answerId = answer.Id });
            }
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }


        return new(answer?.Id, formId, studentId, FormStatus.NotFilled, choosableAnswers, obligatoryAnswers);
    }

    public async Task<IEnumerable<StudentAnswerStatus>> GetStudentFormAnswerStatus(int studentId, CancellationToken? cancellationToken)
    {
        var sql = @"SELECT answer.Id, form.Id AS FormId, Title, ISNULL(FormStatus, 0) AS Status FROM Forms form
LEFT JOIN StudentAnswers answer ON answer.FormId = form.Id AND answer.StudentId = @studentId
";

        try
        {
            return await connection.QueryAsync<StudentAnswerStatus>(
                               new CommandDefinition(sql, new { studentId }, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task UpdateAnswer(int studentId, int formId, FormStatus status, IEnumerable<(string Name, bool IsHidden)> ChoosableAnswers, IEnumerable<(int ObligatoryPreferenceId, int ObligatoryPreferenceOptionId, bool IsHidden)> ObligatoryAnswers, CancellationToken? cancellationToken)
    {
        try
        {
            await DeleteAnswer(formId, studentId, cancellationToken);
            await CreateAnswer(studentId, formId, status, ChoosableAnswers, ObligatoryAnswers, cancellationToken);
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
