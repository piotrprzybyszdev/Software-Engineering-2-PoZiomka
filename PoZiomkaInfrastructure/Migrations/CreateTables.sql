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
	Number int UNIQUE NOT NULL,
	Capacity int NOT NULL
)

CREATE TABLE ApplicationType (
    Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Description TEXT
);

CREATE TABLE Application (
    Id INT PRIMARY KEY IDENTITY(1, 1) NOT NULL,
    StudentId INT NOT NULL,
    ApplicationTypeId INT NOT NULL,
    FileGuid UNIQUEIDENTIFIER NOT NULL,
    Status INT NOT NULL, -- Enum: ApplicationStatus
    FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE CASCADE,
    FOREIGN KEY (ApplicationTypeId) REFERENCES ApplicationType(Id)
);

CREATE TABLE Forms (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    Title NVARCHAR(255) NOT NULL
);

CREATE TABLE ObligatoryPreferences (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    FormId INT NOT NULL,
    [Name] NVARCHAR(255) NOT NULL,
    FOREIGN KEY (FormId) REFERENCES Forms(Id) ON DELETE CASCADE
);

CREATE TABLE ObligatoryPreferenceOptions (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    PreferenceId INT NOT NULL,
    [Name] NVARCHAR(255) NOT NULL,
    FOREIGN KEY (PreferenceId) REFERENCES ObligatoryPreferences(Id) ON DELETE CASCADE
);


CREATE TABLE StudentAnswer (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    FormId INT NOT NULL,
    StudentId INT NOT NULL,
    FOREIGN KEY (FormId) REFERENCES Forms(Id),
    FOREIGN KEY (StudentId) REFERENCES Students(Id)
);

CREATE TABLE StudentAnswerChoosable (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    AnswerId INT NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    IsHidden BIT NOT NULL,
    FOREIGN KEY (AnswerId) REFERENCES StudentAnswer(Id) ON DELETE CASCADE
);

CREATE TABLE StudentAnswerObligatory (
    Id INT PRIMARY KEY IDENTITY(1,1) NOT NULL,
    AnswerId INT NOT NULL,
    ObligatoryPrefernceId INT NOT NULL,
    ObligatoryPreferenceOptionId INT NOT NULL,
    IsHidden BIT NOT NULL,
    FOREIGN KEY (AnswerId) REFERENCES StudentAnswer(Id) ON DELETE CASCADE,
    FOREIGN KEY (ObligatoryPrefernceId) REFERENCES ObligatoryPreferences(Id),
    FOREIGN KEY (ObligatoryPreferenceOptionId) REFERENCES ObligatoryPreferenceOptions(Id)
);
