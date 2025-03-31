using MediatR;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Student.Commands.DeleteStudent;

public class DeleteStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<DeleteStudentCommand>
{
    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await studentRepository.DeleteStudent(request.Id, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new UserNotFoundException("Student not found", e);
        }
    }
}

