INSERT INTO Rooms ([Floor], Number, Capacity) VALUES
(1, 101, 2),
(2, 205, 2),
(3, 311, 2);

INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, RoomId, IsPhoneNumberHidden, IsIndexNumberHidden) VALUES
('student@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 1, 0, 1),
('wojtek@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 1, 0, 1),
('emil@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 2, 0, 1),
('test@gmail.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, NULL, 0, 1);

INSERT INTO Administrators (Email, PasswordHash) VALUES
('admin@example.com', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S');
