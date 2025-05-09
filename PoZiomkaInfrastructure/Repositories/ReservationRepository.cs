using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Reservation;
using PoZiomkaDomain.Reservation.Dtos;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaInfrastructure.Exceptions;
using System.Data;

namespace PoZiomkaInfrastructure.Repositories;

public class ReservationRepository(IDbConnection connection) : IReservationRepository
{
    public async Task<ReservationModel> CreateRoomReservation(int id, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
INSERT INTO Reservations (RoomId, IsAcceptedByAdmin)
OUTPUT Inserted.*
VALUES (@id, 0);
";

        try
        {
            var roomModel = await connection.QuerySingleAsync<ReservationModel>(new CommandDefinition(sqlQuery, new { id }, cancellationToken: cancellationToken ?? default));
            return roomModel;
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<IEnumerable<ReservationModel>> GetAllReservations(CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
SELECT * FROM Reservations
";
        try
        {
            return await connection.QueryAsync<ReservationModel>(sqlQuery);
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task<ReservationDisplay> GetReservationDisplay(int id, CancellationToken? cancellationToken)
    {
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var sqlQueryReservation = @"
SELECT * FROM Reservations WHERE Id = @id
";

        var sqlQueryStudents = @"
SELECT * FROM Students WHERE ReservationId = @id
";

        var sqlQueryRoom = @"
SELECT * FROM Rooms WHERE Id = @id
";

        try
        {
            var reservationModel = await connection.QuerySingleOrDefaultAsync<ReservationModel>(new CommandDefinition(sqlQueryReservation, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            if (reservationModel is null)
            {
                throw new IdNotFoundException();
            }

            var roomModel = await connection.QuerySingleOrDefaultAsync<RoomModel>(new CommandDefinition(sqlQueryRoom, new { id = reservationModel.RoomId }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            if (roomModel is null)
            {
                throw new DomainException("Room assigned to the reservation not found");
            }

            var students = await connection.QueryAsync<StudentModel>(new CommandDefinition(sqlQueryStudents, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));

            return new ReservationDisplay(id, roomModel, students.Select(x => x.ToStudentDisplay(true)), reservationModel.IsAcceptedByAdmin);
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }

    }

    public async Task UpdateReservation(int id, bool isAcceptation, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
UPDATE Reservations
SET IsAcceptedByAdmin = @isAcceptation
WHERE Id = @id
";
        try
        {   
            var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id, isAcceptation }, cancellationToken: cancellationToken ?? default));
            if (rowsAffected == 0)
            {
                throw new IdNotFoundException();
            }
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task UpdateStudentReservation(int id, bool isAcceptation, CancellationToken? cancellationToken)
    {
        var sqlQuery = @"
UPDATE Students
SET HasAcceptedReservation = @isAcceptation
WHERE Id = @id
";
        try
        {
            var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id, isAcceptation }, cancellationToken: cancellationToken ?? default));
            if (rowsAffected == 0)
            {
                throw new IdNotFoundException();
            }
        }
        catch (SqlException exception)
        {
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}
