
using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Commands.Create;

public class CreateCommandHandler(IStudentAnswerRepository studentAnswerRepository, IStudentService studentService) : IRequestHandler<CreateCommand>
{
    public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");
        if (!await studentService.CanFillForm(studentId))
            throw new UserCanNotFillFormException("Student can not fill form");

        // Checking if answers are already filled
        var answerStatus = await studentAnswerRepository.GetStudentFormAnswerStatus(studentId, null);
        if (answerStatus.Any(x => x.Form.Id == request.FormId))
            throw new DomainException("Student has already filled the form");

        await studentAnswerRepository.CreateAnswer(
            studentId, request.FormId, request.ChoosableAnswers, request.ObligatoryAnswers, null);
    }
}
