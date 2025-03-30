using MediatR;
using PoZiomkaApi.Utils;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Admin.Dtos;

namespace PoZiomkaDomain.Admin.Queries.GetAdmin;

public class GetAdminQueryHandler(IAdminRepository adminRepository, IJudgeService judgeService) : IRequestHandler<GetAdminQuery, AdminDisplay>
{
    public async Task<AdminDisplay> Handle(GetAdminQuery request, CancellationToken cancellationToken)
    {
        int loggedInUser = request.User.GetUserId();
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
            throw new StudentNotFoundException($"Student with id `{request.Id}` not found");
        }
    }
}

