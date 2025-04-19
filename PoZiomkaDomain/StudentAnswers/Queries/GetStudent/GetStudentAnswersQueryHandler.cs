

using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Exceptions;


namespace PoZiomkaDomain.StudentAnswers.Queries.GetStudent;

public class GetStudentAnswersQueryHandler(IStudentAnswerRepository studentAnswerRepository, IStudentService studentService) : IRequestHandler<GetStudentAnswersQuery, IEnumerable<StudentAnswerStatus>>
{
    public async Task<IEnumerable<StudentAnswerStatus>> Handle(GetStudentAnswersQuery request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId()??throw new DomainException("Id of the user isn't known");
       
        if(!await studentService.CanFillForm(studentId))
        {
            throw new UserCanNotFillFormException("You can't fill this form");
        }

        return await studentAnswerRepository.GetStudentFormAnswerStatus(studentId, cancellationToken);
    }
}


