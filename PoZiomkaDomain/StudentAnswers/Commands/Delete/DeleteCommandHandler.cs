using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Commands.Delete;

public class DeleteCommandHandler(IStudentRepository studentRepository, IStudentAnswerRepository studentAnswerRepository) : IRequestHandler<DeleteCommand>
{
    public async Task Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");

        var student = await studentRepository.GetStudentById(studentId, cancellationToken);

        if (!student.ToStudentDisplay(false).CanFillForms)
            throw new UserCanNotFillFormException("Student can not fill form");

        var studentAnswers = await studentAnswerRepository.GetStudentAnswerModels(studentId, null);
        if (studentAnswers.All(x => x.Id != request.answerId))
            throw new DomainException("Student has not filled this form or invalid answerId");

        try
        {
            await studentAnswerRepository.DeleteAnswer(request.answerId, null);
        }
        catch (UserDidNotAnswerItException ex)
        {
            throw new DomainException("User does not answered this form so can not delete it", ex);
        }
    }
}

