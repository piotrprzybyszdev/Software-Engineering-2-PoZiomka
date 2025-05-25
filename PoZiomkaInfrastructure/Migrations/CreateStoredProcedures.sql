SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE GenerateMatches
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @availableStudents TABLE (Id INT)

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	BEGIN TRANSACTION

	INSERT INTO @availableStudents
	SELECT DISTINCT(studentId) FROM StudentAnswers answer
	JOIN Students student ON answer.StudentId = student.Id
	WHERE FormStatus = 2 AND student.RoomId IS NULL AND student.ReservationId IS NULL

	INSERT INTO Matches
	SELECT *, 'Pending', 'Pending' FROM @availableStudents as student1
	JOIN @availableStudents as student2 ON student1.Id < student2.Id
	WHERE NOT EXISTS (
		SELECT 1 FROM Matches as mat 
		WHERE mat.StudentId1 = student1.Id AND mat.StudentId2 = student2.Id
	)

	COMMIT TRANSACTION

END
GO


CREATE PROCEDURE GenerateReservations
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @availableStudents TABLE (RowNum INT, Id INT)
	DECLARE @availableRooms TABLE (Id INT, Capacity INT, CapacitySum INT)

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
	BEGIN TRANSACTION

	INSERT INTO @availableStudents
	SELECT ROW_NUMBER() OVER (ORDER BY Id) AS RowNum, Id FROM Students
	WHERE RoomId IS NULL AND ReservationId IS NULL

	INSERT INTO @availableRooms
	SELECT room.Id, Capacity, sum(Capacity) OVER (ORDER BY room.Id) AS CapacitySum
	FROM Rooms AS room
	LEFT JOIN Reservations AS reservation ON reservation.RoomId = room.Id
	WHERE StudentCount = 0 AND reservation.Id IS NULL


	INSERT INTO Reservations (RoomId, IsAcceptedByAdmin)
	SELECT Id, 0
	FROM @availableRooms
	WHERE CapacitySum - Capacity < (SELECT COUNT(1) FROM @availableStudents)

	UPDATE Students
	SET ReservationId = reservation.Id
	FROM @availableStudents student
	JOIN @availableRooms room ON room.CapacitySum > student.RowNum AND student.RowNum > room.CapacitySum - room.Capacity
	JOIN Reservations reservation ON room.Id = reservation.RoomId
	WHERE Students.Id = student.Id

	COMMIT TRANSACTION

END
GO