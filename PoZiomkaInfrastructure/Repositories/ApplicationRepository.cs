using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaInfrastructure.Constants;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;
using PoZiomkaDomain.Application;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaInfrastructure.Repositories;

public class ApplicationRepository(IDbConnection connection) : IApplicationRepository
{
	public Task<ApplicationModel> Get(int applicationId)
	{
		var sqlQuery = @"SELECT * FROM Application WHERE Id = @Id";
		try
		{
			return connection.QuerySingleAsync<ApplicationModel>(sqlQuery, new { Id = applicationId });
		}
		catch (SqlException exception)
		{
			throw new QueryExecutionException(exception.Message, exception.Number);
		}
	}

	public Task<IEnumerable<ApplicationTypeModel>> GetApplicationType(int Id)
	{
		var sqlQuery = @"SELECT * FROM ApplicationType WHERE Id = @Id";
		try
		{
			return connection.QueryAsync<ApplicationTypeModel>(sqlQuery, new { Id });
		}
		catch (SqlException exception)
		{
			throw new QueryExecutionException(exception.Message, exception.Number);
		}
	}

	public Task<IEnumerable<ApplicationModel>> GetAll(int StudentId)
	{
		var sqlQuery = @"SELECT * FROM Application WHERE StudentId = @StudentId";
		try
		{
			return connection.QueryAsync<ApplicationModel>(sqlQuery, new { StudentId });
		}
		catch (SqlException exception)
		{
			throw new QueryExecutionException(exception.Message, exception.Number);
		}
	}

	public Task<IEnumerable<ApplicationModel>> GetAll(string? StudentEmail, string? StudentIndex, int? ApplicationTypeId, ApplicationStatus? ApplicationStatus)
	{
		var sqlQuery = @"SELECT * FROM Application WHERE 1=1";
		if (StudentEmail != null)
			sqlQuery += " AND StudentId IN (SELECT Id FROM Students WHERE Email = @StudentEmail)";
		if (StudentIndex != null)
			sqlQuery += " AND StudentId IN (SELECT Id FROM Students WHERE IndexNumber = @StudentIndex)";
		if (ApplicationTypeId != null)
			sqlQuery += " AND ApplicationTypeId = @ApplicationTypeId";
		if (ApplicationStatus != null)
			sqlQuery += " AND Status = @ApplicationStatus";
		try
		{
			return connection.QueryAsync<ApplicationModel>(sqlQuery, new { StudentEmail, StudentIndex, ApplicationTypeId, ApplicationStatus });
		}
		catch (SqlException exception)
		{
			throw new QueryExecutionException(exception.Message, exception.Number);
		}
	}

	public async Task<IEnumerable<ApplicationTypeModel>> GetTypes()
	{
		var sqlQuery = @"SELECT * FROM ApplicationType";
		try
		{
			return await connection.QueryAsync<ApplicationTypeModel>(sqlQuery);
		}
		catch (SqlException exception)
		{
			throw new QueryExecutionException(exception.Message, exception.Number);
		}
	}

	public async Task Submit(int StudentId, int ApplicationTypeId, Guid FileGuid, ApplicationStatus status)
	{
		var sqlQuery = @"INSERT INTO Application (StudentId, ApplicationTypeId, FileGuid, Status) 
						VALUES (@StudentId, @ApplicationTypeId, @FileGuid, @Status)";
		try
		{
			 await connection.ExecuteAsync(sqlQuery, new { StudentId, ApplicationTypeId, FileGuid, Status = status });
		}
		catch (SqlException exception)
		{
			throw new QueryExecutionException(exception.Message, exception.Number);
		}
	}

	public async Task Update(int applicationId, ApplicationStatus status)
	{
		var sqlQuery = @"UPDATE Application SET Status = @Status WHERE Id = @Id";
		int rowsAffected = 0;
		try
		{
			rowsAffected = await connection.ExecuteAsync(sqlQuery, new { Status = status, Id = applicationId });
		}
		catch (SqlException exception)
		{
			throw new QueryExecutionException(exception.Message, exception.Number);
		}
		if (rowsAffected == 0) throw new IdNotFoundException();
	}
}
