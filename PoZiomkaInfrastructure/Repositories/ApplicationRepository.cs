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

namespace PoZiomkaInfrastructure.Repositories;

public class ApplicationRepository(IDbConnection connection) : IApplicationRepository
{
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
}
