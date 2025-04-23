

using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.StudentAnswers.Commands.Create;

namespace PoZiomkaDomain.StudentAnswers.Commands.Update;

public class UpdateCommandHandler(IStudentAnswerRepository studentAnswerRepository) : IRequestHandler<UpdateCommand>
{
    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");
        await studentAnswerRepository.UpdateAnswer(
            studentId, request.formId, request.ChoosableAnswers, request.ObligatoryAnswers, null);
    }
}
