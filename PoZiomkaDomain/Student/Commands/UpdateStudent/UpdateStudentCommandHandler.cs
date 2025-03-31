using MediatR;
using PoZiomkaApi.Utils;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.UpdateStudent;

public class UpdateStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<UpdateStudentCommand>
{
    public async Task Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
    {
        int loggedInUser = request.User.GetUserId();
        bool isUserAuthorized = request.User.IsInRole(Roles.Administrator) ||
          request.User.IsInRole(Roles.Student) &&
          loggedInUser == request.Id;

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or as an edited student");

        StudentUpdate update = new(request.Id, request.FirstName, request.LastName, request.PhoneNumber, request.IndexNumber, request.IsPhoneNumberHidden, request.IsPhoneNumberHidden);

        try
        {
            await studentRepository.UpdateStudent(update, cancellationToken);
        }
        catch (NoRowsEditedException e)
        {
            throw new StudentNotFoundException("Student not found", e);
        }
    }
}