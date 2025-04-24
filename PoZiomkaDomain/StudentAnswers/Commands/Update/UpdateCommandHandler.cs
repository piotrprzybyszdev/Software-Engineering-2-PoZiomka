

using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.StudentAnswers.Commands.Create;
using PoZiomkaDomain.StudentAnswers.Exceptions;

namespace PoZiomkaDomain.StudentAnswers.Commands.Update;

public class UpdateCommandHandler(IStudentAnswerRepository studentAnswerRepository, IStudentService studentService) : IRequestHandler<UpdateCommand>
{
    public async Task Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        int studentId = request.User.GetUserId() ?? throw new DomainException("UserId is null");
        if (!await studentService.CanFillForm(studentId))
            throw new UserCanNotFillFormException("Student can not fill form");

        await studentAnswerRepository.UpdateAnswer(
            studentId, request.formId, request.ChoosableAnswers, request.ObligatoryAnswers, null);
    }
}
