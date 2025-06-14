﻿using Dapper;
using Microsoft.Data.SqlClient;
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
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var sqlQuery = @"
UPDATE Reservations
SET IsAcceptedByAdmin = @isAcceptation
WHERE Id = @id
";

        var sqlQueryIsAcceptedByStudents = @"
SELECT COUNT(*) FROM Students WHERE ReservationId = @id AND
(HasAcceptedReservation = 0 OR HasAcceptedReservation IS NULL);
";

        var sqlQueryAcceptReservation = @"
UPDATE Rooms SET StudentCount = (SELECT COUNT(*) FROM Students WHERE ReservationId = @id)
WHERE Id = (SELECT RoomId FROM Reservations WHERE Id = @id);
DELETE FROM Matches WHERE
StudentId1 IN (SELECT Id FROM Students WHERE ReservationId = @id) OR
StudentId2 IN (SELECT Id FROM Students WHERE ReservationId = @id);
UPDATE Students SET RoomId = (SELECT RoomId FROM Reservations WHERE Id = @id), ReservationId = NULL, HasAcceptedReservation = NULL
WHERE Students.ReservationId = @id;
DELETE FROM Reservations WHERE Id = @id;
";

        try
        {
            var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id, isAcceptation }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            if (rowsAffected == 0)
            {
                throw new IdNotFoundException();
            }

            if (isAcceptation)
            {
                var pendingStudentAcceptations = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryIsAcceptedByStudents, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));

                if (pendingStudentAcceptations == 0)
                    await connection.ExecuteAsync(new CommandDefinition(sqlQueryAcceptReservation, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            }
            else
            {
                var deleteQuery = "DELETE FROM Reservations WHERE Id = @id;";
                await connection.ExecuteAsync(new CommandDefinition(deleteQuery, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            }

            transaction.Commit();
        }
        catch (SqlException exception)
        {
            transaction.Rollback();
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }

    public async Task UpdateStudentReservation(int id, int studentId, bool isAcceptation, CancellationToken? cancellationToken)
    {
        connection.Open();

        using var transaction = connection.BeginTransaction();

        var sqlQueryReservationAssignedToStudent = @"
SELECT COUNT(*) FROM Students WHERE Id = @studentId AND ReservationId = @id
";

        var sqlQuery = @"
UPDATE Students
SET HasAcceptedReservation = @isAcceptation
WHERE Id = @studentId AND ReservationId = @id
";
        try
        {
            var isReservationAssignedToStudent = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryReservationAssignedToStudent, new { studentId, id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            if (isReservationAssignedToStudent == 0)
            {
                transaction.Rollback();
                throw new DomainException("Reservation is not assigned to the student");
            }

            var rowsAffected = await connection.ExecuteAsync(new CommandDefinition(sqlQuery, new { id, studentId, isAcceptation }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            if (rowsAffected == 0)
            {
                transaction.Rollback();
                throw new IdNotFoundException();
            }

            if (!isAcceptation)
            {
                var sqlQueryDeleteReservation = @"
UPDATE Students Set ReservationId = NULL WHERE ReservationId = (SELECT ReservationId FROM Students WHERE Id = @id);
DELETE FROM Reservations WHERE Id = @id;
";

                var rowsAffectedDelete = await connection.ExecuteAsync(new CommandDefinition(sqlQueryDeleteReservation, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
                if (rowsAffectedDelete == 0)
                {
                    transaction.Rollback();
                    throw new IdNotFoundException();
                }
            }
            else
            {
                var sqlQueryIsAcceptedByAdmin = @"
SELECT IsAcceptedByAdmin FROM Reservations reservation
WHERE reservation.Id = @id;";

                var sqlQueryIsAcceptedByStudents = @"
SELECT COUNT(*) FROM Students student
WHERE student.ReservationId = 1
AND (student.HasAcceptedReservation = 0 OR student.HasAcceptedReservation IS NULL)
";
                var isAcceptedByAdmin = await connection.ExecuteScalarAsync<bool>(new CommandDefinition(sqlQueryIsAcceptedByAdmin, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
                var pendingStudentAcceptations = await connection.ExecuteScalarAsync<int>(new CommandDefinition(sqlQueryIsAcceptedByStudents, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));

                var sqlDeleteReservation = @"
UPDATE Rooms SET StudentCount = (SELECT COUNT(*) FROM Students WHERE ReservationId = @id)
WHERE Id = (SELECT RoomId FROM Reservations WHERE Id = @id);
DELETE FROM Matches WHERE
StudentId1 IN (SELECT Id FROM Students WHERE ReservationId = @id) OR
StudentId2 IN (SELECT Id FROM Students WHERE ReservationId = @id);
UPDATE Students SET RoomId = (SELECT RoomId FROM Reservations WHERE Id = @id), ReservationId = NULL, HasAcceptedReservation = NULL
WHERE Students.ReservationId = @id;
DELETE FROM Reservations WHERE Id = @id;";

                if (pendingStudentAcceptations == 0 && isAcceptedByAdmin)
                    await connection.ExecuteAsync(new CommandDefinition(sqlDeleteReservation, new { id }, transaction: transaction, cancellationToken: cancellationToken ?? default));
            }
            transaction.Commit();
        }
        catch (SqlException exception)
        {
            transaction.Rollback();
            throw new QueryExecutionException(exception.Message, exception.Number);
        }
    }
}