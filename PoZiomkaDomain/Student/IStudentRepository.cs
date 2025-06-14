﻿using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student;

public class EmailNotUniqueException : Exception;

public interface IStudentRepository
{
    public Task RegisterStudent(StudentRegister studentCreate, CancellationToken? cancellationToken);
    public Task<StudentModel> GetStudentById(int id, CancellationToken? cancellationToken);
    public Task<StudentModel> GetStudentByEmail(string email, CancellationToken? cancellationToken);
    public Task<IEnumerable<StudentModel>> GetStudentsByRoomId(int roomId, CancellationToken? cancellationToken);
    public Task<IEnumerable<StudentModel>> GetAllStudents(CancellationToken? cancellationToken);
    public Task CreateStudent(StudentCreate studentCreate, CancellationToken? cancellationToken);
    public Task UpdateStudent(StudentUpdate studentEdit, CancellationToken? cancellationToken);
    public Task UpdateReservation(int studentId, int reservationId, bool? HasAccepted, CancellationToken? cancellationToken);
    public Task ConfirmStudent(StudentConfirm studentConfirm, CancellationToken? cancellationToken);
    public Task DeleteStudent(int id, CancellationToken? cancellationToken);
    public Task UpdatePassword(PasswordUpdate passwordUpdate, CancellationToken? cancellationToken);
}
