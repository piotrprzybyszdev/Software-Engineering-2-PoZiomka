using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Commands.Create;

public class CreateCommandHandler(IStudentRepository studentRepository, IFormRepository formRepository, IStudentAnswerRepository studentAnswerRepository) : IRequestHandler<CreateCommand>
{
    public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");

        var student = await studentRepository.GetStudentById(studentId, cancellationToken);

        if (!student.ToStudentDisplay(false).CanFillForms)
            throw new UserCanNotFillFormException("Student can not fill form");

        var form = await formRepository.GetFormDisplay(request.FormId, cancellationToken);

        // Checking if answers are already filled
        var answerStatus = await studentAnswerRepository.GetStudentFormAnswerStatus(studentId, null);
        if (answerStatus.Single(answer => answer.Form.Id == request.FormId).Status != FormStatus.NotFilled)
            throw new DomainException("Answer for this form already exists");

        await studentAnswerRepository.CreateAnswer(
            studentId, request.FormId, form.ObligatoryPreferences.Count() == request.ObligatoryAnswers.Count() ? FormStatus.Filled : FormStatus.InProgress, request.ChoosableAnswers, request.ObligatoryAnswers, null);
    }
}
