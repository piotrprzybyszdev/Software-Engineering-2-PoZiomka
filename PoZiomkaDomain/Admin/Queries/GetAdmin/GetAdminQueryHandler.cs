using MediatR;
using PoZiomkaDomain.Admin.Dtos;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Admin.Queries.GetAdmin;

public class GetAdminQueryHandler(IAdminRepository adminRepository) : IRequestHandler<GetAdminQuery, AdminDisplay>
{
    public async Task<AdminDisplay> Handle(GetAdminQuery request, CancellationToken cancellationToken)
    {
        bool isUserAuthorized = request.User.IsInRole(Roles.Administrator);

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or a student that has a match with the student");

        try
        {
            var admin = await adminRepository.GetAdminById(request.Id, cancellationToken);
            return admin.ToAdminDisplay();
        }
        catch
        {
            throw new UserNotFoundException($"Student with id `{request.Id}` not found");
        }
    }
}

