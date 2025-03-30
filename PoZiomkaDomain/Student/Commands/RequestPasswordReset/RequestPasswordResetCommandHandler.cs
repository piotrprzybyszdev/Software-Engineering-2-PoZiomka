using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Student.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandHandler(IStudentRepository studentRepository, IEmailService emailService) : IRequestHandler<RequestPasswordResetCommand>
{
    public async Task Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        if (await studentRepository.GetStudentByEmail(request.Email, cancellationToken) == null)
            throw new StudentNotFoundException(request.Email);

        await emailService.SendPasswordResetEmail(request.Email);
    }
}
