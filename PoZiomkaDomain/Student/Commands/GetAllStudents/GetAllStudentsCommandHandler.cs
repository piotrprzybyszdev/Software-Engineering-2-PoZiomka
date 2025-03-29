using MediatR;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.GetAllStudents;

public class GetAllStudentsCommandHandler(IStudentRepository studentRepository) : IRequestHandler<GetAllStudentsCommand, IEnumerable<StudentDisplay>>
{
    public async Task<IEnumerable<StudentDisplay>> Handle(GetAllStudentsCommand request, CancellationToken cancellationToken)
    {
        var students = await studentRepository.GetAllStudents();
        return students.Select(s => s.ToStudentDisplay(false));
    }
}