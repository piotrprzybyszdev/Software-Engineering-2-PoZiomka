using MediatR;
using PoZiomkaDomain.Student.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.Student.Commands.EditStudent;

public record EditStudentCommand(StudentEdit studentEdit, ClaimsPrincipal User) : IRequest;