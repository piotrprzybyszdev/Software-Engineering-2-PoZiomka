using Dapper;
using Microsoft.Data.SqlClient;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaInfrastructure.Constants;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class RoomRepository(IDbConnection connection) : IRoomRepository
{
    public async Task CreateRoom(RoomCreate roomCreate, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
INSERT INTO Rooms ([Floor], Number, Capacity,StudentCount)
VALUES (@floor, @number, @capacity,0);
";

        try
        {
            await connection.ExecuteAsync(new CommandDefinition(sqlQuery, roomCreate, cancellationToken: cancellationToken ?? default));
        }
        catch (SqlException exception)
        when (exception.Number == ErrorNumbers.UniqueConstraintViolation)
        {
            throw new RoomNumberNotUniqueException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<RoomDisplay> GetRoomById(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
SELECT room.*, reservation.Id AS reservationId FROM Rooms as room
LEFT JOIN Reservations as reservation ON room.Id = reservation.RoomId
WHERE room.Id = @id
";

        try
        {
            var room = await connection.QuerySingleOrDefaultAsync<RoomDisplay>(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
            return room ?? throw new IdNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<IEnumerable<RoomDisplay>> GetAllRooms(CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
SELECT room.*, reservation.Id AS reservationId FROM Rooms as room
LEFT JOIN Reservations as reservation ON room.Id = reservation.RoomId
";
        try
        {
            return await connection.QueryAsync<RoomDisplay>(new CommandDefinition(sqlQuery, cancellationToken: cancellationToken ?? default));
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

    public async Task RemoveStudent(int id, int studentId, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
BEGIN TRANSACTION

UPDATE Students
SET RoomId = NULL
WHERE Id = @studentId

UPDATE Rooms
SET StudentCount = StudentCount - 1
WHERE Id = @id

COMMIT TRANSACTION
";
        try
        {
            var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id, studentId }, cancellationToken: cancellationToken ?? default));
            if (rowsAffected == 0) throw new IdNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task AddStudent(int id, int studentId, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
BEGIN TRANSACTION

UPDATE Students
SET RoomId = @id
WHERE Id = @studentId

UPDATE Rooms
SET StudentCount = StudentCount + 1
WHERE Id = @id

COMMIT TRANSACTION
";

        try
        {
            var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id, studentId }, cancellationToken: cancellationToken ?? default));
            if (rowsAffected == 0) throw new IdNotFoundException();
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
