using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Commands.Update;

public class UpdateCommandHandler(IStudentRepository studentRepository, IStudentAnswerRepository studentAnswerRepository) : IRequestHandler<UpdateCommand>
{
    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");

        var student = await studentRepository.GetStudentById(studentId, cancellationToken);

        if (!student.ToStudentDisplay(false).CanFillForms)
            throw new UserCanNotFillFormException("Student can not fill form");

        await studentAnswerRepository.UpdateAnswer(
            studentId, request.formId, request.ChoosableAnswers, request.ObligatoryAnswers, null);
    }
}
