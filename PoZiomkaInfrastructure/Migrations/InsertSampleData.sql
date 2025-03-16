INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, IsPhoneNumberHidden, IsIndexNumberHidden)  
VALUES ('student@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) +'WPie2EYhZWwisuXyTqpKCO9jXclGJQIZl5IZ3mDjLZdXWIQHZaK0K', 1, '123-456-789', 'S12345', NULL, NULL, 0, 1);
INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, IsPhoneNumberHidden, IsIndexNumberHidden)  
VALUES ('wojtek@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) +'WPie2EYhZWwisuXyTqpKCO9jXclGJQIZl5IZ3mDjLZdXWIQHZaK0K', 1, '123-456-789', 'S12345', NULL, NULL, 0, 1);
INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, IsPhoneNumberHidden, IsIndexNumberHidden)  
VALUES ('emil@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) +'WPie2EYhZWwisuXyTqpKCO9jXclGJQIZl5IZ3mDjLZdXWIQHZaK0K', 1, '123-456-789', 'S12345', NULL, NULL, 0, 1);

INSERT INTO Admin (Email, PasswordHash)  
VALUES ('admin@example.com', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) +'WPie2EYhZWwisuXyTqpKCO9jXclGJQIZl5IZ3mDjLZdXWIQHZaK0K');
