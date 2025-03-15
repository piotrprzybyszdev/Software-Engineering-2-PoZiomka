INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, IsPhoneNumberHidden, IsIndexNumberHidden)  
VALUES ('student@example.com', 'John', 'Doe', '0x1234567890ABCDEF', 1, '123-456-789', 'S12345', NULL, NULL, 0, 1);

INSERT INTO Admin (Email, PasswordHash)  
VALUES ('admin@example.com', '0x1234567890ABCDEF');
