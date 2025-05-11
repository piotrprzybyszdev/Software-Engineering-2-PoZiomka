using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Communication;
using PoZiomkaDomain.Communication.Dtos;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class CommunicationRepository(IDbConnection connection) : ICommunicationRepository
{
    public async Task CreateCommunications(IEnumerable<int> studentIds, string description, CancellationToken? cancellationToken)
    {
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var sqlQuery = @"
INSERT INTO Communications (StudentId, Description)
VALUES (@studentId, @description);
";

        var sqlQueryStudentExists = @"
SELECT COUNT(*) FROM Students WHERE Id = @studentId
";

        try
        {
            foreach (var studentId in studentIds)
            {
                var studentExists = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryStudentExists, new { studentId }, transaction: transaction, cancellationToken: cancellationToken ?? default));
                if (studentExists == 0)
                {
                    throw new IdNotFoundException();
                }

                await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { studentId, description }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            }
        }
        catch (SqlException exception)
        {
            transaction.Rollback();
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        transaction.Commit();
    }

    public async Task DeleteCommunication(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
DELETE FROM Communications WHERE Id = @id
";

        try
        {
            var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
            if (rowsAffected == 0)
            {
                throw new IdNotFoundException();
            }
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<IEnumerable<CommunicationDisplay>> GetStudentCommunications(int studentId, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
SELECT Id, Description FROM Communications WHERE StudentId = @studentId
";

        var sqlQueryStudentExists = @"
SELECT COUNT(*) FROM Students WHERE Id = @studentId
";

        try
        {
            var studentExists = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryStudentExists, new { studentId }, cancellationToken: cancellationToken ?? default));
            if (studentExists == 0)
            {
                throw new IdNotFoundException();
            }

            return await connection.QueryAsync<CommunicationDisplay>(new CommandDefinition(sqlQuery, new { studentId }, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<int> GetStudentIdByCommunicationId(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
SELECT StudentId FROM Communications WHERE Id = @id
";

        var sqlQueryCommunicationExists = @"
SELECT COUNT(*) FROM Communications WHERE Id = @id
";

        try
        {
            var communicationExists = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryCommunicationExists, new { id }, cancellationToken: cancellationToken ?? default));
            if (communicationExists == 0)
            {
                throw new IdNotFoundException();
            }

            return await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
