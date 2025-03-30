using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student;

public class EmailNotUniqueException : Exception;

public class EmailNotFoundException : Exception;

public interface IStudentRepository
{
    public Task CreateStudent(StudentCreate studentCreate, CancellationToken? cancellationToken);
    public Task<StudentModel> GetStudentById(int id, CancellationToken? cancellationToken);
    public Task<StudentModel> GetStudentByEmail(string email, CancellationToken? cancellationToken);
    public Task<IEnumerable<StudentModel>> GetAllStudents(CancellationToken? cancellationToken);
	public Task EditStudent(StudentEdit studentEdit, CancellationToken? cancellationToken);
	public Task ConfirmStudent(StudentConfirm studentConfirm, CancellationToken? cancellationToken);
    public Task DeleteStudent(int id, CancellationToken? cancellationToken);
    public Task ResetPassword(PasswordUpdate passwordUpdate, CancellationToken? cancellationToken);
}
