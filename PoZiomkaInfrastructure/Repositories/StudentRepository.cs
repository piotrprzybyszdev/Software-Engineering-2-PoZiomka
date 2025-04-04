﻿using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Exceptions;
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
VALUES (@email, @passwordHash, @isConfirmed);
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
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }


    public async Task<StudentModel> GetStudentById(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"SELECT * FROM Students WHERE id = @id";

        try
        {
            var student = await connection.QuerySingleOrDefaultAsync<StudentModel>(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
            return student ?? throw new QueryExecutionException("Student not found", id);
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }


    public async Task<StudentModel> GetStudentByEmail(string email, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"SELECT * FROM Students WHERE Email = @email";

        try
        {
            var student = await connection.QuerySingleOrDefaultAsync<StudentModel>(new CommandDefinition(sqlQuery, new { email }, cancellationToken: cancellationToken ?? default));
            return student ?? throw new EmailNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<IEnumerable<StudentModel>> GetAllStudents(CancellationToken? cancellationToken)
    {
        var sqlQuery = @"SELECT * FROM Students";

        try
        {
            return await connection.QueryAsync<StudentModel>(new CommandDefinition(sqlQuery, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task ConfirmStudent(StudentConfirm studentConfirm, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
UPDATE Students SET IsConfirmed = 1 WHERE Email = @email;
";

        int rowsAffected;
        try
        {
            rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, studentConfirm, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

        if (rowsAffected == 0) throw new EmailNotFoundException();
    }
    public async Task EditStudent(StudentEdit studentEdit, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
UPDATE Students
SET
	FirstName = @FirstName,
	LastName = @LastName,
	PhoneNumber = @PhoneNumber,
	IndexNumber = @IndexNumber,
	IsPhoneNumberHidden = @IsPhoneNumberHidden,
	IsIndexNumberHidden = @IsIndexNumberHidden
WHERE id = @id
";
        int rowsAffected;
        try
        {
            rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, studentEdit, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
        if (rowsAffected == 0) throw new NoRowsEditedException("User not found");
    }

    public async Task DeleteStudent(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"DELETE FROM Students WHERE id = @id";

        int rowsAffected;
        try
        {
            rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
        if (rowsAffected == 0) throw new NoRowsEditedException("No rows was deleted");
    }

    public async Task ResetPassword(PasswordUpdate passwordUpdate, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
UPDATE Students
SET PasswordHash = @PasswordHash
WHERE Email = @Email
";
        int rowsAffected;
        try
        {
            rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, passwordUpdate, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
        if (rowsAffected == 0) throw new NoRowsEditedException("User not found");
    }
}
