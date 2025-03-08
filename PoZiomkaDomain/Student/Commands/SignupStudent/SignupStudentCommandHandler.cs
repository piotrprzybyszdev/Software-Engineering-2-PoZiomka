using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.SignupStudent;

public class SignupStudentCommandHandler(IPasswordService passwordService, IStudentRepository studentRepository) : IRequestHandler<SignupStudentCommand>
{
    public async Task Handle(SignupStudentCommand request, CancellationToken cancellationToken)
    {
        StudentCreate studentCreate = new(request.Email, passwordService.ComputeHash(request.Password));

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
