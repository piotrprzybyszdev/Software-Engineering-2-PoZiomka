﻿using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Admin;
using PoZiomkaDomain.Admin.Dtos;
using PoZiomkaDomain.Exceptions;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class AdminRepository(IDbConnection connection) : IAdminRepository
{
    public async Task<AdminModel> GetAdminById(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"SELECT * FROM Admin WHERE Id = @id";

        try
        {
            var admin = await connection.QuerySingleOrDefaultAsync<AdminModel>(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
            return admin ?? throw new AdminNotFoundException("Admin not found");
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

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
