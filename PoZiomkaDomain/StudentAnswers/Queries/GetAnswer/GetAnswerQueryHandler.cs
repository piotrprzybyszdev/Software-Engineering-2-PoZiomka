

using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.StudentAnswers.Dtos;

namespace PoZiomkaDomain.StudentAnswers.Queries.GetAnswer;
public class GetAnswerQueryHandler(IStudentAnswerRepository studentAnswerRepository, IMatchRepository matchRepository) : IRequestHandler<GetAnswerQuery, StudentAnswerDisplay>
{
    public async Task<StudentAnswerDisplay> Handle(GetAnswerQuery request, CancellationToken cancellationToken)
    {
        int studentId = request.user.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        bool authorized = false;
        if (await matchRepository.IsMatch(studentId, request.studentId))
        {
            authorized = true;
        }
        if (studentId == request.studentId)
        {
            authorized = true;
        }
        if (!authorized)
        {
            throw new UnauthorizedAccessException("You are not authorized to see this answer");
        }
        var result = await studentAnswerRepository.GetStudentAnswer(request.formId, request.studentId, cancellationToken);

        if (studentId != request.studentId)
        {
            result = result.HideAnswers();
        }
        return result;
    }
}


