using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaDomain.Student.Commands.ConfirmStudent;

public class ConfirmStudentCommandHandler(IStudentRepository studentRepository) : IRequestHandler<ConfirmStudentCommand>
{
    public async Task Handle(ConfirmStudentCommand request, CancellationToken cancellationToken)
    {
        StudentConfirm studentConfirm = new(request.Email);

        try
        {
            await studentRepository.ConfirmStudent(studentConfirm, cancellationToken);
        }
        catch (EmailNotFoundException)
        {
            throw new EmailNotRegisteredException($"User with email `{request.Email}` not registered");
        }
    }
}
