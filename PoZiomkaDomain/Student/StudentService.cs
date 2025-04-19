namespace PoZiomkaDomain.Student;

public class StudentService : IStudentService
{
    public async Task<bool> CanFillForm(int studentId)
    {
        return true;
    }
}
