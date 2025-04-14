using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student.Exceptions;

namespace PoZiomkaDomain.Student.Commands.DeleteStudent;

public class DeleteStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<DeleteStudentCommand>
{
    public async Task Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
    {
        int loggedInUser = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        bool isUserAuthorized = request.User.IsInRole(Roles.Administrator) ||
          request.User.IsInRole(Roles.Student) &&
          loggedInUser == request.Id;

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or as the student to delete");

        try
        {
            await studentRepository.DeleteStudent(request.Id, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new StudentNotFoundException("Student not found", e);
        }
    }
}

