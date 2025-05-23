﻿using MediatR;
using PoZiomkaDomain.Admin.Dtos;
using PoZiomkaDomain.Admin.Exceptions;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Admin.Queries.GetAdmin;

public class GetAdminQueryHandler(IAdminRepository adminRepository) : IRequestHandler<GetAdminQuery, AdminDisplay>
{
    public async Task<AdminDisplay> Handle(GetAdminQuery request, CancellationToken cancellationToken)
    {
        bool isUserAuthorized = request.User.IsInRole(Roles.Administrator);
        int adminId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or a student that has a match with the student");

        try
        {
            var admin = await adminRepository.GetAdminById(adminId, cancellationToken);
            return admin.ToAdminDisplay();
        }
        catch
        {
            throw new AdminNotFoundException($"Admin with id `{adminId}` not found");
        }
    }
}

