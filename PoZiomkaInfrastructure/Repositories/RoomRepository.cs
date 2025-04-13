using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Dtos;
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

    public async Task<RoomModel> GetRoomById(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"SELECT * FROM Rooms WHERE id = @id";

        try
        {
            var room = await connection.QuerySingleOrDefaultAsync<RoomModel>(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
            return room ?? throw new IdNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<IEnumerable<RoomModel>> GetAllRooms(CancellationToken? cancellationToken)
    {
        var sqlQuery = @"SELECT * FROM Rooms";
        try
        {
            var rooms = await connection.QueryAsync<RoomModel>(new CommandDefinition(sqlQuery, cancellationToken: cancellationToken ?? default));
            return rooms;
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task DeleteRoom(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"DELETE FROM Rooms WHERE id = @id";
        try
        {
            var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
            if (rowsAffected == 0) throw new IdNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
