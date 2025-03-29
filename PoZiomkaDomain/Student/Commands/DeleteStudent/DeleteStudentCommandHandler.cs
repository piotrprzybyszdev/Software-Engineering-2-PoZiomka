using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;
using System.Runtime.Serialization;

namespace PoZiomkaDomain.Student.Commands.EditStudent;

public class DeleteStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<DeleteStudentCommand>
{ 
	public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
	{
		try
		{
			await studentRepository.DeleteStudent(request.Id);
		}
		catch (NoRowsEditedException e)
		{
			throw new StudentNotFoundException("Student not found", e);
		}
	}
}

