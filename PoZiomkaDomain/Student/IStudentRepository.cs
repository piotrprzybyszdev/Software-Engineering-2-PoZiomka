using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student;

public class EmailNotUniqueException : Exception;

public class EmailNotFoundException : Exception;

public interface IStudentRepository
{
    public Task CreateStudent(StudentCreate studentCreate, CancellationToken? cancellationToken);
    public Task ConfirmStudent(StudentConfirm studentConfirm, CancellationToken? cancellationToken);
}
