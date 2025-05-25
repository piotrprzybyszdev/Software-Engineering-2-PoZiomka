using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Match;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Services;

public class JudgeService(IDbConnection connection) : IJudgeService
{
    public async Task FindMatching(CancellationToken? cancellationToken)
    {
        try
        {
            await connection.ExecuteAsync("EXEC GenerateMatches;", cancellationToken ?? default);
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task GenerateReservations(CancellationToken? cancellationToken)
    {
        try
        {
            await connection.ExecuteAsync("EXEC GenerateReservations;", cancellationToken ?? default);
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
