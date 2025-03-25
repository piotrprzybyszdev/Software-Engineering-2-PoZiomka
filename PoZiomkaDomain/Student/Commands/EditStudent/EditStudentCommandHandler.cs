using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.EditStudent;

public class EditStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<EditStudentCommand>
{
	public async Task Handle(EditStudentCommand request, CancellationToken cancellationToken)
	{
		try
		{
			await studentRepository.EditStudent(request.studentEdit);
		}
		catch (Exception e)
		{
			throw new NoRowEditedException("Error while editing student", e);
		}
	}
}