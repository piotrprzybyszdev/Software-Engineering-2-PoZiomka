using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student.Exceptions;

namespace PoZiomkaDomain.Student.Queries.GetStudent;

public class GetStudentQueryHandler(IStudentRepository studentRepository, IMatchRepository matchRepository) : IRequestHandler<GetStudentQuery, StudentDisplay>
{
    public async Task<StudentDisplay> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");
        int userId = request.Id ?? loggedInUserId;

        bool isUserAuthorized = request.User.IsInRole(Roles.Administrator) ||
          request.User.IsInRole(Roles.Student) &&
          (loggedInUserId == userId || await matchRepository.IsMatch(loggedInUserId, userId));

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or a student that has a match with the student");

        var hidePersonalInfo = !(request.User.IsInRole(Roles.Administrator) || loggedInUserId == userId);

        try
        {
            var student = await studentRepository.GetStudentById(userId, cancellationToken);
            return student.ToStudentDisplay(hidePersonalInfo);
        }
        catch
        {
            throw new StudentNotFoundException($"Student with id `{userId}` not found");
        }
    }
}

