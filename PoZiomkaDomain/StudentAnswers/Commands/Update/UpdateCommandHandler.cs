using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Commands.Update;

public class UpdateCommandHandler(IStudentRepository studentRepository, IFormRepository formRepository, IStudentAnswerRepository studentAnswerRepository) : IRequestHandler<UpdateCommand>
{
    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");

        var student = await studentRepository.GetStudentById(studentId, cancellationToken);

        if (!student.ToStudentDisplay(false).CanFillForms)
            throw new UserCanNotFillFormException("Student can not fill form");

        var form = await formRepository.GetFormDisplay(request.formId, cancellationToken);

        await studentAnswerRepository.UpdateAnswer(
            studentId, request.formId, form.ObligatoryPreferences.Count() == request.ObligatoryAnswers.Count() ? FormStatus.Filled : FormStatus.InProgress, request.ChoosableAnswers, request.ObligatoryAnswers, null);
    }
}
