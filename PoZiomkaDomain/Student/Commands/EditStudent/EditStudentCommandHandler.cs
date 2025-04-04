﻿using MediatR;
using PoZiomkaApi.Utils;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Student.Commands.EditStudent;

public class EditStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<EditStudentCommand>
{
    public async Task Handle(EditStudentCommand request, CancellationToken cancellationToken)
    {
        int loggedInUser = request.User.GetUserId();
        bool isUserAuthorized = request.User.IsInRole(Roles.Administrator) ||
          (request.User.IsInRole(Roles.Student) &&
          (loggedInUser == request.studentEdit.Id));

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or as an edited student");

        try
        {
            await studentRepository.EditStudent(request.studentEdit, cancellationToken);
        }
        catch (NoRowsEditedException e)
        {
            throw new StudentNotFoundException("Student not found", e);
        }
    }
}