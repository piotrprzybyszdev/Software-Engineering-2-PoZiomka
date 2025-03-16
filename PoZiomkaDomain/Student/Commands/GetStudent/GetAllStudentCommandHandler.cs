using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.GetStudent;

public class GetAllStudentsCommandHandler(IStudentRepository studentRepository) : IRequestHandler<GetAllStudentsCommand, IEnumerable<StudentDisplay>>
{
	public async Task<IEnumerable<StudentDisplay>> Handle(GetAllStudentsCommand request, CancellationToken cancellationToken)
	{
		var students = await studentRepository.GetAllStudents();
		return students.Select(s => s.ToStudentDisplay(false));
	}
}