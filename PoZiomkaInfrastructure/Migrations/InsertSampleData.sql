INSERT INTO Rooms ([Floor], Number, Capacity) VALUES
(1, 101, 2),
(2, 205, 2),
(3, 311, 2);

INSERT INTO Administrators (Email, PasswordHash)
VALUES ('admin@example.com', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S');

INSERT INTO ApplicationType (Name, Description) VALUES
('Scholarship', 'Application for scholarship programs'),
('Exchange Program', 'Application for student exchange programs'),
('Internship', 'Application for internship opportunities'),
('Thesis Submission', 'Application for thesis submission and approval'),
('Graduation Request', 'Application to request graduation approval');

INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, RoomId, IsPhoneNumberHidden, IsIndexNumberHidden) VALUES
('student@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 1, 0, 1),
('wojtek@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 1, 0, 1),
('emil@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 2, 0, 1),
('test@gmail.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, NULL, 0, 1);

INSERT INTO Application (StudentId, ApplicationTypeId, FileGuid, Status) VALUES
(1, 1, NEWID(), 0),
(1, 2, NEWID(), 1),
(1, 3, NEWID(), 2),
(1, 4, NEWID(), 0),
(1, 5, NEWID(), 1),

(2, 1, NEWID(), 0),
(2, 2, NEWID(), 0),
(2, 3, NEWID(), 1),
(2, 4, NEWID(), 2),
(2, 5, NEWID(), 0),

(3, 1, NEWID(), 1),
(3, 2, NEWID(), 2),
(3, 3, NEWID(), 0),
(3, 4, NEWID(), 1),
(3, 5, NEWID(), 0);