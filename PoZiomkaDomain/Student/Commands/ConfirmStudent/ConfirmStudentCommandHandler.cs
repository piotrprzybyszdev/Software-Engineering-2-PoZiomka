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
        ClaimsIdentity claimsIdentity;
        try
        {
            claimsIdentity = await jwtService.ReadToken(request.Token);
        }
        catch (Exception e)
        {
            throw new TokenValidationException("Error validating token", e);
        }

        var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email) ?? throw new TokenValidationException("No email claim in token");

        var email = emailClaim.Value;

        var studentConfirm = new StudentConfirm(email);        

        try
        {
            await studentRepository.ConfirmStudent(studentConfirm, cancellationToken);
        }
        catch (EmailNotFoundException e)
        {
            throw new EmailNotRegisteredException($"User with email `{email}` not registered", e);
        }
    }
}
