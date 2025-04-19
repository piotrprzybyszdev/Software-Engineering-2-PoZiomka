
using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Commands.Create;

public class CreateCommandHandler(IStudentAnswerRepository studentAnswerRepository) : IRequestHandler<CreateCommand>
{
    public async Task Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");

        // Checking if answers are already filled
        var answerStatus = await studentAnswerRepository.GetStudentFormAnswerStatus(studentId, null);
        if (answerStatus.Any(x => x.Form.Id == request.FormId))
            throw new DomainException("Student has already filled the form");

        await studentAnswerRepository.CreateAnswer(
            studentId, request.FormId, request.ChoosableAnswers, request.ObligatoryAnswers, null);
    }
}
