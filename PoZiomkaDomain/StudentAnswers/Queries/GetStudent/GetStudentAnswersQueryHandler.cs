using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Queries.GetStudent;

public class GetStudentAnswersQueryHandler(IStudentRepository studentRepository, IMatchRepository matchRepository, IStudentAnswerRepository studentAnswerRepository) : IRequestHandler<GetStudentAnswersQuery, IEnumerable<StudentAnswerStatus>>
{
    public async Task<IEnumerable<StudentAnswerStatus>> Handle(GetStudentAnswersQuery request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");

        var student = await studentRepository.GetStudentById(request.StudentId, cancellationToken);

        if (studentId == request.StudentId && !student.ToStudentDisplay(false).CanFillForms)
            throw new UserCanNotFillFormException("Student can not fill form");

        if (studentId != request.StudentId && !await matchRepository.IsMatch(studentId, request.StudentId))
            throw new UnauthorizedException("User must be logged in as an administrator or a student that has a match with the student");
        var a= await studentAnswerRepository.GetStudentFormAnswerStatus(request.StudentId, cancellationToken);
        return a;
    }
}
