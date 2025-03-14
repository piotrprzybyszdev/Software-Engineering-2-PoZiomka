using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.SignupStudent;

public class SignupStudentCommandHandler(IPasswordService passwordService, IStudentRepository studentRepository, IJwtService jwtService, IEmailService emailService) : IRequestHandler<SignupStudentCommand>
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

        var token = await jwtService.GenerateToken(new ClaimsIdentity([new Claim(ClaimTypes.Email, request.Email)]), TimeSpan.FromMinutes(20));
        
        await emailService.SendEmail(request.Email, "[PoZiomka] Confirm your email", token);
    }
}
