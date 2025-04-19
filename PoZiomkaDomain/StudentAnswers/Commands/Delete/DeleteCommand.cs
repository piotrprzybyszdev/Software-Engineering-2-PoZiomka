
using MediatR;
using System.Security.Claims;

namespace PoZiomkaDomain.StudentAnswers.Commands.Delete;

public record DeleteCommand(ClaimsPrincipal User, int formId) : IRequest;