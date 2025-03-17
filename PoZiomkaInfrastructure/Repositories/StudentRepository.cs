using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaInfrastructure.Constants;
using PoZiomkaInfrastructure.Exceptions;
using PoZiomkaDomain.Exceptions;
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
			throw new QueryExceutionException(exception.Message, exception.Number);
		}
	}


	public async Task<StudentModel> GetStudentById(int id, CancellationToken? cancellationToken)
	{
		var sqlQuery = @"SELECT * FROM Students WHERE id = @id";

		try
		{
			var student = await connection.QuerySingleOrDefaultAsync<StudentModel>(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
			return student ?? throw new QueryExceutionException("Student not found", id);
		}
		catch (SqlException exception)
		{
			throw new QueryExceutionException(exception.Message, exception.Number);
		}
	}

	public async Task<IEnumerable<StudentModel>> GetAllStudents()
	{
		var sqlQuery = @"SELECT * FROM Students";

		try
		{
			return await connection.QueryAsync<StudentModel>(sqlQuery);
		}
		catch (SqlException exception)
		{
			throw new QueryExceutionException(exception.Message, exception.Number);
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
			throw new QueryExceutionException(exception.Message, exception.Number);
		}

		if (rowsAffected == 0) throw new EmailNotFoundException();
	}
	public async Task EditStudent(StudentEdit studentEdit)
	{
		var sqlQuery = @"UPDATE Students SET
FirstName=@FirstName, LastName=@LastName, PhoneNumber=@PhoneNumber,
IndexNumber=@IndexNumber, IsPhoneNumberHidden=@IsPhoneNumberHidden,
IsIndexNumberHidden=@IsIndexNumberHidden WHERE id=@id";
		int rowsAffedted;
		try
		{
			rowsAffedted = await connection.ExecuteAsync(sqlQuery, studentEdit);
		}
		catch (SqlException exception)
		{
			throw new QueryExceutionException(exception.Message, exception.Number);
		}
		if (rowsAffedted == 0) throw new NoRowEditedException("User not found");
	}

	public async Task DeleteStudent(int id)
	{
		var sqlQuery = @"DELETE FROM Students WHERE id = @id";

		int rowAffected;
		try
		{
			rowAffected=await connection.ExecuteAsync(sqlQuery, new { id });
		}
		catch (SqlException exception)
		{
			throw new QueryExceutionException(exception.Message, exception.Number);
		}
		if (rowAffected == 0) throw new NoRowEditedException("Now row was deleted");
	}
}
