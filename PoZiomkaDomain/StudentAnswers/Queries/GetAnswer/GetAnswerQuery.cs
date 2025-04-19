
using MediatR;
using PoZiomkaDomain.StudentAnswers.Dtos;
using System.Security.Claims;

namespace PoZiomkaDomain.StudentAnswers.Queries.GetAnswer;
public record GetAnswerQuery(ClaimsPrincipal user,int formId, int studentId):IRequest<StudentAnswerDisplay>;

