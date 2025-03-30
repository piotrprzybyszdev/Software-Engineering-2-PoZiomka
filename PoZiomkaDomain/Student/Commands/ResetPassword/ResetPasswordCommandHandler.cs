using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.ResetPassword;

public class ResetPasswordCommandHandler(IJwtService jwtService, IPasswordService passwordService, IStudentRepository studentRepository) : IRequestHandler<ResetPasswordCommand>
{
    public async Task Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
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
            ?? throw new ClaimNotFoundException("Email claim not found");

        var email = emailClaim.Value;
        var passwordHash = passwordService.ComputeHash(request.Password);

        try
        {
            await studentRepository.ResetPassword(new PasswordUpdate(email, passwordHash), cancellationToken);
        }
        catch (NoRowsEditedException)
        {
            throw new StudentNotFoundException($"Can't reset password for student with email `{email}` the account doesn't exist");
        }
    }
}
