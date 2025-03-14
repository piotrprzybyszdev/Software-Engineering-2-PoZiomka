using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.ConfirmStudent;

public class ConfirmStudentCommandHandler(IStudentRepository studentRepository, IJwtService jwtService) : IRequestHandler<ConfirmStudentCommand>
{
    public async Task Handle(ConfirmStudentCommand request, CancellationToken cancellationToken)
    {
        var claimsIdentity = await jwtService.ReadToken(request.Token);

        var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email) ?? throw new Exception("No email in claims");

        var email = emailClaim.Value;

        var studentConfirm = new StudentConfirm(email);        

        try
        {
            await studentRepository.ConfirmStudent(studentConfirm, cancellationToken);
        }
        catch (EmailNotFoundException)
        {
            throw new EmailNotRegisteredException($"User with email `{email}` not registered");
        }
    }
}
