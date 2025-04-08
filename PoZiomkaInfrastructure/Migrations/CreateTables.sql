CREATE TABLE Students (
	Id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	Email varchar(100) UNIQUE NOT NULL,
	FirstName varchar(100) NULL,
	LastName varchar(100) NULL,
	PasswordHash varchar(60),
	IsConfirmed bit NOT NULL,
	PhoneNumber varchar(9) NULL,
	IndexNumber varchar(6) NULL,
	ReservationId int NULL,
	HasAcceptedReservation bit NULL,
	RoomId int NULL,
	IsPhoneNumberHidden bit NOT NULL DEFAULT 1,
	IsIndexNumberHidden bit NOT NULL DEFAULT 1,
);

CREATE TABLE Administrators(
	Id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	Email varchar(100) UNIQUE NOT NULL,
	PasswordHash varchar(60) NOT NULL
);

CREATE TABLE Rooms (
	Id int PRIMARY KEY IDENTITY(1, 1) NOT NULL,
	[Floor] int NOT NULL,
	Number int NOT NULL,
	Capacity int NOT NULL
)