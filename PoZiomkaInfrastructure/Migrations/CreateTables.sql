CREATE TABLE Students (
	Id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	Email varchar(100) UNIQUE NOT NULL,
	FirstName varchar(100) NOT NULL,
	LastName varchar(100) NOT NULL,
	PasswordHash binary(60) NOT NULL,
	IsConfirmed bit NOT NULL,
	PhoneNumber varchar(20) NULL,
	IndexNumber varchar(20) NULL,
	ReservationId int NULL,
	HasAcceptedReservation bit NULL,
	IsPhoneNumberHidden bit NOT NULL,
	IsIndexNumberHidden bit NOT NULL,
);