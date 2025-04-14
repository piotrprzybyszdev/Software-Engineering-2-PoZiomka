using MediatR;
using PoZiomkaDomain.Admin.Dtos;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Common.Interface;
using PoZiomkaDomain.Student.Exceptions;
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
            throw new StudentNotFoundException($"Admin with email {request.Email} not registered", e);
        }

        if (!passwordService.VerifyHash(request.Password, Admin.PasswordHash))
            throw new InvalidPasswordException($"Password for user with email {request.Email} is invalid");

        IEnumerable<Claim> claims = [
            new(ClaimTypes.NameIdentifier, Admin.Id.ToString()),
            new(ClaimTypes.Role, Roles.Administrator)
        ];

        return claims;
    }
}
