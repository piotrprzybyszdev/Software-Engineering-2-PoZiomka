using MediatR;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Queries.GetAllStudents;

public class GetAllStudentsCommandHandler(IStudentRepository studentRepository) : IRequestHandler<GetAllStudentsQuery, IEnumerable<StudentDisplay>>
{
    public async Task<IEnumerable<StudentDisplay>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        var students = await studentRepository.GetAllStudents(cancellationToken);
        return students.Select(s => s.ToStudentDisplay(false));
    }
}