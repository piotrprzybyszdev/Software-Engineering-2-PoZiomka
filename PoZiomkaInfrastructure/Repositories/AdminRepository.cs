using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Admin;
using PoZiomkaDomain.Admin.Dtos;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class AdminRepository(IDbConnection connection) : IAdminRepository
{
    public async Task<AdminModel> GetAdminByEmail(string email, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"SELECT * FROM Admin WHERE Email = @email";

        try
        {
            var admin = await connection.QuerySingleOrDefaultAsync<AdminModel>(new CommandDefinition(sqlQuery, new { email }, cancellationToken: cancellationToken ?? default));
            return admin ?? throw new EmailNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
