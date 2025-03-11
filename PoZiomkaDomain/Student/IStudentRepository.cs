using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student;

public class EmailNotUniqueException : Exception;

public interface IStudentRepository
{
    public Task CreateStudent(StudentCreate studentCreate, CancellationToken? cancellationToken);
}
