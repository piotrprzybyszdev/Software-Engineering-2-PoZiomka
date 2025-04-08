using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student;
using PoZiomkaInfrastructure.Constants;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class RoomRepository(IDbConnection connection) : IRoomRepository
{
    public async Task CreateRoom(RoomCreate roomCreate, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
INSERT INTO Rooms ([Floor], Number, Capacity)
VALUES (@floor, @number, @capacity);
";

        try
        {
            await connection.ExecuteAsync(new CommandDefinition(sqlQuery, roomCreate, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
