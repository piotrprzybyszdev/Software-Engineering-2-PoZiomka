
namespace PoZiomkaDomain.Student;
public interface IStudentService
{
    public Task<bool> CanFillForm(int studentId);
}

