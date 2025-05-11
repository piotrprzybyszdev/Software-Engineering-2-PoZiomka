using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Match.Dtos;

namespace PoZiomkaDomain.Match.Queries.StudentMatchesQuery;

public class StudentMatchQuaryHandler(IMatchRepository matchRepository) : IRequestHandler<StudentMatchQuery, IEnumerable<MatchModel>>
{
    public async Task<IEnumerable<MatchModel>> Handle(StudentMatchQuery request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");
        return await matchRepository.GetStudentMatches(studentId);
    }
}
