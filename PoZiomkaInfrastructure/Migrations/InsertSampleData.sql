INSERT INTO Rooms ([Floor], Number, Capacity) VALUES
(1, 101, 2),
(2, 205, 2),
(3, 311, 2);

INSERT INTO Administrators (Email, PasswordHash)
VALUES ('admin@example.com', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S');

INSERT INTO Students (Email, FirstName, LastName, PasswordHash, IsConfirmed, PhoneNumber, IndexNumber, ReservationId, HasAcceptedReservation, RoomId, IsPhoneNumberHidden, IsIndexNumberHidden) VALUES
('student@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 1, 0, 1),
('wojtek@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 1, 0, 1),
('emil@example.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, 2, 0, 1),
('test@gmail.com', 'John', 'Doe', CHAR(36)+'2a'+CHAR(36)+'11' + CHAR(36) + 'SsGgl/8bovY2WSslaj/9o.EcfoDnIn20gpIYeXOqcUOWyiYGPvD0S', 1, '123456789', '123456', NULL, NULL, NULL, 0, 1);

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

INSERT INTO Forms (Title) VALUES 
('University Application Form'),
('Scholarship Preference Form'),
('Housing Preference Form');


-- FormId: 1 (University Application Form)
INSERT INTO ObligatoryPreferences (FormId, Name) VALUES 
(1, 'Preferred Campus'),
(1, 'Intended Major'),
(1, 'Mode of Study'),
(1, 'Entry Term'),
(1, 'Preferred Language of Instruction');

-- Preferred Campus (PreferenceId = 1)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(1, 'Main Campus'),
(1, 'City Campus'),
(1, 'Online Campus');

-- Intended Major (PreferenceId = 2)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(2, 'Computer Science'),
(2, 'Business Administration'),
(2, 'Psychology'),
(2, 'Mechanical Engineering');

-- Mode of Study (PreferenceId = 3)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(3, 'Full-Time'),
(3, 'Part-Time'),
(3, 'Evening Classes');

-- Entry Term (PreferenceId = 4)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(4, 'Fall 2025'),
(4, 'Spring 2026'),
(4, 'Summer 2026');

-- Preferred Language of Instruction (PreferenceId = 5)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(5, 'English'),
(5, 'German'),
(5, 'Spanish');

-- FormId: 2
INSERT INTO ObligatoryPreferences (FormId, Name) VALUES 
(2, 'Scholarship Type'),
(2, 'Financial Need Level'),
(2, 'Academic Excellence'),
(2, 'Extracurricular Involvement'),
(2, 'Recommendation Source');

-- Scholarship Type (PreferenceId = 6)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(6, 'Merit-Based'),
(6, 'Need-Based'),
(6, 'Athletic Scholarship');

-- Financial Need Level (PreferenceId = 7)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(7, 'Low'),
(7, 'Medium'),
(7, 'High');

-- Academic Excellence (PreferenceId = 8)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(8, 'Top 5% of Class'),
(8, 'Top 10% of Class'),
(8, 'Honors');

-- Extracurricular Involvement (PreferenceId = 9)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(9, 'Very Active'),
(9, 'Moderately Active'),
(9, 'Not Active');

-- Recommendation Source (PreferenceId = 10)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(10, 'Teacher'),
(10, 'Counselor'),
(10, 'Coach');

-- FormId: 3 (Housing Preference Form)
INSERT INTO ObligatoryPreferences (FormId, Name) VALUES 
(3, 'Roommate Preference'),
(3, 'Dormitory Style'),
(3, 'Noise Tolerance'),
(3, 'Wake-up Time'),
(3, 'Cleanliness Preference');

-- Roommate Preference (11)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(11, 'Same Major'),
(11, 'No Preference'),
(11, 'Night Owl');

-- Dormitory Style (12)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(12, 'Single Room'),
(12, 'Shared Room'),
(12, 'Suite');

-- Noise Tolerance (13)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(13, 'Very Quiet'),
(13, 'Moderate'),
(13, 'Lively');

-- Wake-up Time (14)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(14, 'Early (5-7 AM)'),
(14, 'Mid (7-9 AM)'),
(14, 'Late (9+ AM)');

-- Cleanliness Preference (15)
INSERT INTO ObligatoryPreferenceOptions (PreferenceId, Name) VALUES 
(15, 'Neat Freak'),
(15, 'Average'),
(15, 'Messy but Functional');


-- Insert 4 StudentAnswers
INSERT INTO StudentAnswer (FormId, StudentId) VALUES 
(1, 1),
(2, 1),
(3, 1);


INSERT INTO StudentAnswerChoosable (AnswerId, Name, IsHidden) VALUES 
(1, 'Joined Art Club', 0),
(1, 'Volunteered at School Events', 0),
(1, 'Won Debate Championship', 0);


-- Preferred Campus (PreferenceId = 1) → 'Main Campus' (OptionId = 1)
INSERT INTO StudentAnswerObligatory (AnswerId, ObligatoryPrefernceId, ObligatoryPreferenceOptionId, IsHidden) VALUES 
(1, 1, 1, 0),
(1, 2, 4, 0),
(1, 3, 7, 0),
(1, 4, 10, 0),
(1, 5, 13, 0),
(2, 6, 16, 0),
(2, 9, 22, 0);
