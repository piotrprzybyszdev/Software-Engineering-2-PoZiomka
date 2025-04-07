using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Common.Interface;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.LoginStudent;

public class LoginStudentCommandHandler(IPasswordService passwordService, IStudentRepository studentRepository) : IRequestHandler<LoginStudentCommand, IEnumerable<Claim>>
{
    public async Task<IEnumerable<Claim>> Handle(LoginStudentCommand request, CancellationToken cancellationToken)
    {
        StudentModel student;
        try
        {
            student = await studentRepository.GetStudentByEmail(request.Email, cancellationToken);
        }
        catch (EmailNotFoundException e)
        {
            throw new UserNotFoundException($"Student with email `{request.Email}` not registered", e);
        }

        if (!student.IsConfirmed)
            throw new EmailNotConfirmedException($"Email `{request.Email}` is not confirmed");

        if (student.PasswordHash == null)
            throw new PasswordNotSetException($"Password for user with email `{request.Email}` not set");

        if (!passwordService.VerifyHash(request.Password, student.PasswordHash))
            throw new InvalidPasswordException($"Password for user with email `{request.Email}` is invalid");

        IEnumerable<Claim> claims = [
            new(ClaimTypes.NameIdentifier, student.Id.ToString()),
            new(ClaimTypes.Role, Roles.Student)
        ];

        return claims;
    }
}
