using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class FormRepository(IDbConnection connection) : IFormRepository
{
    public async Task CreateForm(FormCreate form, CancellationToken? cancellationToken)
    {
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var sqlQueryForm = @"
INSERT INTO Forms (Title)
OUTPUT INSERTED.Id
VALUES (@title);
";
        var sqlQueryObligatoryPreference = @"
INSERT INTO ObligatoryPreferences (FormId, Name)
OUTPUT INSERTED.Id
VALUES (@formId, @name);
";
        var sqlQueryObligatoryPreferenceOption = @"
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name)
VALUES (@preferenceId, @name);
";

        try
        {
            var formId = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryForm, new { title = form.Title }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            foreach (var obligatoryPreference in form.ObligatoryPreferences)
            {
                var preferenceId = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryObligatoryPreference, new { formId, name = obligatoryPreference.Name }, transaction: transaction, cancellationToken: cancellationToken ?? default));
                foreach (var option in obligatoryPreference.Options)
                {
                    await connection.ExecuteAsync(new CommandDefinition(sqlQueryObligatoryPreferenceOption, new { preferenceId, name = option }, transaction: transaction, cancellationToken: cancellationToken ?? default));
                }
            }
        }
        catch (SqlException exception)
        {
            transaction.Rollback();
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        transaction.Commit();
    }

    public async Task DeleteForm(int id, CancellationToken? cancellationToken)
    {
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var sqlQueryAnswersDelete = @"DELETE FROM StudentAnswers WHERE FormId = @id";

        try
        {
            await connection.ExecuteAsync(new CommandDefinition(sqlQueryAnswersDelete, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            transaction.Rollback();
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        var sqlQueryFormDelete = @"DELETE FROM Forms WHERE Id = @id";

        int rowsAffected;
        try
        {
            rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQueryFormDelete, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            transaction.Rollback();
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
        if (rowsAffected == 0) throw new IdNotFoundException();

        transaction.Commit();
    }

    public async Task<FormDisplay> GetFormDisplay(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
SELECT f.Id, f.Title, op.Id, op.Name, opo.Id, opo.Name
FROM Forms f
LEFT JOIN ObligatoryPreferences op ON f.Id = op.FormId
LEFT JOIN ObligatoryPreferenceOptions opo ON op.Id = opo.PreferenceId
WHERE f.Id = @id
";

        IEnumerable<FormDisplayList> forms;
        try
        {
            forms = await connection.QueryAsync<FormDisplayList, ObligatoryPreferenceDisplayList, ObligatoryPreferenceOptionDisplay, FormDisplayList>(
                new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default),
                (form, preference, option) =>
                {
                    if (preference is not null)
                    {
                        form.ObligatoryPreferences.Add(preference);
                        if (option is not null)
                            preference.Options.Add(option);
                    }
                    return form;
                },
                splitOn: "Id, Id");
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        if (forms.Count() == 0) throw new IdNotFoundException();

        var form = forms.First();

        form.ObligatoryPreferences = forms.Where(f1 => f1.ObligatoryPreferences.Count > 0).GroupBy(f1 => f1.ObligatoryPreferences.First().Id).Select(f1 =>
        {
            var groupedObligatoryPreference = f1.First().ObligatoryPreferences.First();
            groupedObligatoryPreference.Options = f1.Where(f2 => f2.ObligatoryPreferences.First().Options.Count > 0).Select(f2 => f2.ObligatoryPreferences.First().Options.First()).ToList();
            return groupedObligatoryPreference;
        }).ToList();

        return form.ToFormDisplay();
    }

    public async Task<IEnumerable<FormModel>> GetForms(CancellationToken? cancellationToken)
    {
        var sqlQuery = @"SELECT * FROM Forms";

        try
        {
            return await connection.QueryAsync<FormModel>(new CommandDefinition(sqlQuery, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task UpdateForm(FormUpdate form, CancellationToken? cancellationToken)
    {
        connection.Open();

        var sqlQueryFormExists = @"
SELECT COUNT(*) FROM Forms WHERE Id = @id
";
        var formExists = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryFormExists, new { id = form.Id }, cancellationToken: cancellationToken ?? default));
        if (formExists == 0) throw new IdNotFoundException();

        using var transaction = connection.BeginTransaction();

        var sqlQueryForm = @"
UPDATE Forms
SET Title = @title
WHERE Id = @id;
";
        var sqlQueryClearObligatoryPreferences = @"
DELETE FROM ObligatoryPreferences
WHERE FormId = @formId;
";
        var sqlQueryObligatoryPreference = @"
INSERT INTO ObligatoryPreferences (FormId, Name)
OUTPUT INSERTED.Id
VALUES (@formId, @name);
";
        var sqlQueryObligatoryPreferenceOption = @"
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name)
VALUES (@preferenceId, @name);
";

        try
        {
            await connection.ExecuteAsync(new CommandDefinition(sqlQueryForm, new { title = form.Title, id = form.Id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            await connection.ExecuteAsync(new CommandDefinition(sqlQueryClearObligatoryPreferences, new { formId = form.Id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            foreach (var obligatoryPreference in form.ObligatoryPreferences)
            {
                var preferenceId = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryObligatoryPreference, new { formId = form.Id, name = obligatoryPreference.Name }, transaction: transaction, cancellationToken: cancellationToken ?? default));
                foreach (var option in obligatoryPreference.Options)
                {
                    await connection.ExecuteAsync(new CommandDefinition(sqlQueryObligatoryPreferenceOption, new { preferenceId, name = option }, transaction: transaction, cancellationToken: cancellationToken ?? default));
                }
            }
        }
        catch (SqlException exception)
        {
            transaction.Rollback();
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        transaction.Commit();
    }
}
