using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaInfrastructure.Constants;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class StudentRepository(IDbConnection connection) : IStudentRepository
{
    public async Task CreateStudent(StudentCreate studentCreate, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
INSERT INTO Students (Email, PasswordHash, IsConfirmed)
VALUES (@email, CAST(@passwordHash AS BINARY), 0);
";

        try
        {
            await connection.ExecuteAsync(new CommandDefinition(sqlQuery, studentCreate, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        when (exception.Number == ErrorNumbers.UniqueConstraintViolation)
        {
            throw new EmailNotUniqueException();
        }
        catch (SqlException exception)
        {
            throw new QueryExceutionException(exception.Message, exception.Number);
        }
    }
}
