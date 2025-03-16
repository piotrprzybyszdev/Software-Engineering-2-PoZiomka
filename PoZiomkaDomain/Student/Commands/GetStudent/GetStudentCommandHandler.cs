using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.GetStudent;

public class GetStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<GetStudentCommand, StudentDisplay>
{
	public async Task<StudentDisplay> Handle(GetStudentCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var student = await studentRepository.GetStudentById(request.Id, cancellationToken);
			return student.ToStudentDisplay(request.Hide);
		}
		catch
		{
			throw new NotFound($"Student with id `{request.Id}` not found");
		}
	}
}

public class GetAllStudentsCommandHandler(IStudentRepository studentRepository) : IRequestHandler<GetAllStudentsCommand, IEnumerable<StudentDisplay>>
{
	public async Task<IEnumerable<StudentDisplay>> Handle(GetAllStudentsCommand request, CancellationToken cancellationToken)
	{
		var students = await studentRepository.GetAllStudents();
		return students.Select(s => s.ToStudentDisplay(false));
	}
}