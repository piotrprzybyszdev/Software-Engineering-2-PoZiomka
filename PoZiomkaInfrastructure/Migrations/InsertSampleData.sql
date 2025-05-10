INSERT INTO Rooms ([Floor], Number, Capacity) VALUES
(1, 101, 2),
(2, 205, 2),
(3, 311, 2);

-- Reservations table
INSERT INTO Reservations (RoomId, IsAcceptedByAdmin)
VALUES
(1, 0),
(2, 0);

INSERT INTO Administrators (Email, PasswordHash)
VALUES ('admin@example.com', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S');

INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, RoomId, IsPhoneNumberHidden, IsIndexNumberHidden) VALUES
('student@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', 1, 1, NULL, 0, 1),
('wojtek@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', 1, NULL, NULL, 0, 1),
('emil@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, NULL, 0, 1),
('test@gmail.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 2, 0, 1);

INSERT INTO Application (StudentId, ApplicationTypeId, FileGuid, Status) VALUES
(1, 1, 'b6f4cfe3-fc21-45fc-95b5-3dd29ea4de4f', 0),
(1, 2, 'c1186e5b-e8e1-4b5b-8ae3-1e0bc4464b97', 1),
(1, 3, 'fb72b7be-0d92-402c-85de-43a8b66becc5', 2),
(1, 1, '1cb850e0-57f4-4b12-a6a2-239e2b774cb3', 0),
(1, 2, '2f1a2a6b-5c6c-4877-91e4-d7e3e7f7f3cc', 1),

(2, 1, 'cad3a4c7-baf9-4f35-91c4-4f84e03f1e79', 0),
(2, 2, 'ed53c9d4-0cb7-4aa6-9b4d-9f029c775d71', 0),
(2, 3, '41caa1aa-9ae8-4c94-8875-9871c08b81b2', 1),
(2, 1, '04fa14e2-83c4-4f1c-9f62-2ac03e24e195', 2),
(2, 1, '3cdd89e6-308e-4422-9475-431a87e75a3d', 0),

(3, 1, '6f44d9d6-0d50-49ff-8d75-5dbb6e80bc7f', 1),
(3, 2, '2a83c5cf-37c7-4f4d-b0ea-291502ebd435', 2),
(3, 3, 'cf82c9b4-64db-4d0b-8d9d-9ef832e9a574', 0),
(3, 1, 'ae41fadd-93c9-48a7-bdd3-7dd2fd70f845', 1),
(3, 1, '3f373a47-93dc-4f07-b438-18835d27aeb1', 0);

INSERT INTO Forms (Title)
VALUES ('Podstawowa ankieta dotycząca współlokatora');

INSERT INTO ObligatoryPreferences (FormId, Name)
VALUES 
(1, 'Czy wolisz dzielić pokój z osobą tej samej płci?'),                     -- ID = 1
(1, 'Jakie cechy są dla Ciebie najważniejsze u współlokatora?'),           -- ID = 2
(1, 'Jakie godziny aktywności Ci odpowiadają?'),                           -- ID = 3
(1, 'Czy ważne jest dla Ciebie, aby współlokator był studentem tego samego kierunku?'), -- ID = 4
(1, 'Czy współlokator powinien przestrzegać ciszy nocnej?');               -- ID = 5

-- Preference ID = 1
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name)
VALUES 
(1, 'Tak'),
(1, 'Nie'),
(1, 'Nie mam preferencji');

-- Preference ID = 2
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name)
VALUES 
(2, 'Czystość i porządek'),
(2, 'Towarzyskość'),
(2, 'Cichy tryb życia'),
(2, 'Wspólne zainteresowania'),
(2, 'Brak nałogów (np. palenie, alkohol)');

-- Preference ID = 3
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name)
VALUES 
(3, 'Poranne (6:00–10:00)'),
(3, 'Dziennie (10:00–18:00)'),
(3, 'Wieczorne (18:00–24:00)'),
(3, 'Nocne (24:00–6:00)');

-- Preference ID = 4
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name)
VALUES 
(4, 'Tak'),
(4, 'Nie'),
(4, 'Nie mam preferencji');

-- Preference ID = 5
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name)
VALUES 
(5, 'Zdecydowanie tak'),
(5, 'Raczej tak'),
(5, 'Raczej nie'),
(5, 'Zdecydowanie nie');

INSERT INTO StudentAnswers (FormId, StudentId, FormStatus)
VALUES 
(1, 1, 2),
(1, 2, 1),
(1, 3, 0);

INSERT INTO StudentAnswersChoosable (AnswerId, Name, IsHidden)
VALUES 
(1, 'Lubię piłkę nożną', 0),
(1, 'Lubię chodzić na siłownię', 0),
(1, 'Lubię grać w gry komputerowe', 0);

INSERT INTO StudentAnswersObligatory (AnswerId, ObligatoryPreferenceId, ObligatoryPreferenceOptionId, IsHidden)
VALUES
(1, 1, 1, 0),
(1, 2, 5, 0),
(1, 3, 11, 0),
(1, 4, 14, 0),
(1, 5, 17, 0),
(2, 1, 2, 0),
(2, 2, 4, 0),
(2, 3, 12, 0);

-- Mathces table
INSERT INTO Matches (StudentId1, StudentId2, Status1, Status2)
VALUES (3, 2, 'Accepted', 'Pending');

INSERT INTO Matches (StudentId1, StudentId2, Status1, Status2)
VALUES (1, 3, 'Rejected', 'Rejected');

INSERT INTO Matches (StudentId1, StudentId2, Status1, Status2)
VALUES (1, 4, 'Accepted', 'Accepted');

INSERT INTO Matches (StudentId1, StudentId2, Status1, Status2)
VALUES (2, 1, 'Pending', 'Pending');

-- Communications table
INSERT INTO Communications (StudentId, [Description])
VALUES
(1, 'Zostałeś zakwaterowany w pokoju 101'),
(1, 'Twój wniosek został odrzucony'),
(1, 'Zostałeś wydalony z akademika'),
(2, 'Wygrałeś iPhone 16 Pro Max'),
(3, 'Zostałeś zakwaterowany w pokoju 311'),
(4, 'Zostałeś zakwaterowany w pokoju 205'),
(4, 'Twój wniosek został odrzucony');
