using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student;

public class EmailNotUniqueException : Exception;

public interface IStudentRepository
{
    public Task CreateStudent(StudentCreate studentCreate, CancellationToken? cancellationToken);
    public Task<StudentModel> GetStudentById(int id, CancellationToken? cancellationToken);
    public Task<IEnumerable<StudentModel>> GetAllStudents();
}
