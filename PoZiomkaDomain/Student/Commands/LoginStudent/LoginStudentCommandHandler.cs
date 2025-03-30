using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;
using System.Net.Http;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.LoginStudent;

public class LoginStudentCommandHandler(IPasswordService passwordService, IStudentRepository studentRepository, IEmailService emailService) : IRequestHandler<LoginStudentCommand, IEnumerable<Claim>>
{
    public async Task<IEnumerable<Claim>> Handle(LoginStudentCommand request, CancellationToken cancellationToken)
    {
        StudentModel student;
        try
        {
            student = await studentRepository.GetStudentByEmail(request.Email, cancellationToken);
        }
        catch (EmailNotRegisteredException)
        {
            throw new EmailNotRegisteredException($"User with email `{request.Email}` not found");
        }

        IEnumerable<Claim> claims = [
            new(ClaimTypes.NameIdentifier, student.Id.ToString()),
            new(ClaimTypes.Role, Roles.Student)
        ];

        return claims;
    }
}
