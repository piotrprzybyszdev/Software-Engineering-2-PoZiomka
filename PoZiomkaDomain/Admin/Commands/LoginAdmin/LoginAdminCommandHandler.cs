using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Admin.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Admin.Commands.LoginAdmin;

public class LoginAdminCommandHandler(IPasswordService passwordService, IAdminRepository adminRepository) : IRequestHandler<LoginAdminCommand, IEnumerable<Claim>>
{
    public async Task<IEnumerable<Claim>> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        AdminModel Admin;
        try
        {
            Admin = await adminRepository.GetAdminByEmail(request.Email, cancellationToken);
        }
        catch (EmailNotFoundException e)
        {
            throw new InvalidCredentialsException($"Admin with email {request.Email} not registered", e);
        }

        if (!passwordService.VerifyHash(request.Password, Admin.PasswordHash))
        {
            throw new InvalidCredentialsException("Invalid password");
        }

        IEnumerable<Claim> claims = [
            new(ClaimTypes.NameIdentifier, Admin.Id.ToString()),
            new(ClaimTypes.Role, Roles.Administrator)
        ];

        return claims;
    }
}
