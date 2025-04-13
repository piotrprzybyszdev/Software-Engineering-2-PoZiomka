using MediatR;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.CreateStudent;

public class CreateStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<CreateStudentCommand>
{
    public async Task Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        StudentCreate studentCreate = new(request.Email, request.FirstName, request.LastName, request.IndexNumber, request.PhoneNumber);

        try
        {
            await studentRepository.CreateStudent(studentCreate, cancellationToken);
        }
        catch (EmailNotUniqueException)
        {
            throw new EmailTakenException($"User with email `{request.Email}` already exists");
        }
    }
}
