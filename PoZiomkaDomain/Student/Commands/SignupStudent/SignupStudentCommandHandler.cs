using MediatR;
using PoZiomkaDomain.Common.Interface;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.SignupStudent;

public class SignupStudentCommandHandler(IPasswordService passwordService, IStudentRepository studentRepository, IEmailService emailService) : IRequestHandler<SignupStudentCommand>
{
    public async Task Handle(SignupStudentCommand request, CancellationToken cancellationToken)
    {
        StudentRegister studentRegister = new(request.Email, passwordService.ComputeHash(request.Password));

        try
        {
            await studentRepository.RegisterStudent(studentRegister, cancellationToken);
        }
        catch (EmailNotUniqueException)
        {
            throw new EmailTakenException($"User with email `{request.Email}` already exists");
        }

        await emailService.SendEmailConfirmationEmail(request.Email);
    }
}
