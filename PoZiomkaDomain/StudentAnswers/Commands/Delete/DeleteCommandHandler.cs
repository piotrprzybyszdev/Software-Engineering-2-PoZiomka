

using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Commands.Delete;
public class DeleteCommandHandler(IStudentAnswerRepository studentAnswerRepository, IStudentService studentService) : IRequestHandler<DeleteCommand>
{
    public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");
        if(!await studentService.CanFillForm(studentId))
            throw new UserCanNotFillFormException("Student can not fill form");

        try
        {
            await studentAnswerRepository.DeleteAnswer(request.formId, studentId, null);
        }
        catch (UserDidNotAnswerItException ex)
        {
            throw new DomainException("User does not answered this form so can not delete it", ex);
        }
        
    }
}

