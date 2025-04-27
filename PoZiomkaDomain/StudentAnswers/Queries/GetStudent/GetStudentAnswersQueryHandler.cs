using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Queries.GetStudent;

public class GetStudentAnswersQueryHandler(IStudentRepository studentRepository, IStudentAnswerRepository studentAnswerRepository) : IRequestHandler<GetStudentAnswersQuery, IEnumerable<StudentAnswerStatus>>
{
    public async Task<IEnumerable<StudentAnswerStatus>> Handle(GetStudentAnswersQuery request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");

        var student = await studentRepository.GetStudentById(studentId, cancellationToken);

        if (!student.ToStudentDisplay(false).CanFillForms)
            throw new UserCanNotFillFormException("Student can not fill form");

        return await studentAnswerRepository.GetStudentFormAnswerStatus(studentId, cancellationToken);
    }
}
