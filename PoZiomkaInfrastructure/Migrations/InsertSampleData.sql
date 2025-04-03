INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, RoomId, IsPhoneNumberHidden, IsIndexNumberHidden)  
VALUES ('student@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123-456-789', 'S12345', NULL, NULL, NULL, 0, 1);
INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, RoomId, IsPhoneNumberHidden, IsIndexNumberHidden)  
VALUES ('wojtek@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123-456-789', 'S12345', NULL, NULL, NULL, 0, 1);
INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, RoomId, IsPhoneNumberHidden, IsIndexNumberHidden)  
VALUES ('emil@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123-456-789', 'S12345', NULL, NULL, NULL, 0, 1);

INSERT INTO Administrators (Email, PasswordHash)
VALUES ('admin@example.com', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S');
