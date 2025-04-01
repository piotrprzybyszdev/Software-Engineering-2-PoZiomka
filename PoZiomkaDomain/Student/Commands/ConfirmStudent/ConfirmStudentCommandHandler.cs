using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Common.Interface;
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
        catch (NotATokenException e)
        {
            throw new InvalidTokenException("Given string is not a token", e);
        }
        catch (TokenExpiredException e)
        {
            throw new InvalidTokenException("Token has expired", e);
        }
        catch (TokenValidationException e)
        {
            throw new InvalidTokenException("Token wasn't issued by this authority", e);
        }

        var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email)
            ?? throw new InvalidTokenException("Email claim not found in token");

        var email = emailClaim.Value;

        var studentConfirm = new StudentConfirm(email);

        try
        {
            await studentRepository.ConfirmStudent(studentConfirm, cancellationToken);
        }
        catch (EmailNotFoundException e)
        {
            throw new UserNotFoundException($"User with email `{email}` not registered", e);
        }
    }
}
