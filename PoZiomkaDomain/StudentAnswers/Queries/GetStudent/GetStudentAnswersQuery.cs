using MediatR;
using PoZiomkaDomain.StudentAnswers.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.StudentAnswers.Queries.GetStudent;
public record GetStudentAnswersQuery(ClaimsPrincipal User) :IRequest<IEnumerable<StudentAnswerStatus>>;
