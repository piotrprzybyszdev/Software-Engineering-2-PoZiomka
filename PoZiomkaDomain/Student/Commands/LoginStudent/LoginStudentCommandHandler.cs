﻿using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.LoginStudent;

public class LoginStudentCommandHandler(IPasswordService passwordService, IStudentRepository studentRepository) : IRequestHandler<LoginStudentCommand, IEnumerable<Claim>>
{
    public async Task<IEnumerable<Claim>> Handle(LoginStudentCommand request, CancellationToken cancellationToken)
    {
        StudentModel student;
        try
        {
            student = await studentRepository.GetStudentByEmail(request.Email, cancellationToken);
        }
        catch (EmailNotFoundException e)
        {
            throw new InvalidCredentialsException($"User with email {request.Email} not registered", e);
        }

        if (student.PasswordHash == null)
            throw new PasswordNotSet($"Password for user with email {request.Email} not set");

        if (!passwordService.VerifyHash(request.Password, student.PasswordHash))
            throw new InvalidCredentialsException($"Password for user with email {request.Email} is invalid");

        IEnumerable<Claim> claims = [
            new(ClaimTypes.NameIdentifier, student.Id.ToString()),
            new(ClaimTypes.Role, Roles.Student)
        ];

        return claims;
    }
}
