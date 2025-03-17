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
		catch (Exception e)
		{
			throw new DeleteException("Error while deleting student", e);
		}
	}
}

