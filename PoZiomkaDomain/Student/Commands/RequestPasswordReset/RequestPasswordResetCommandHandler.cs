using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Common.Interface;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Student.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandHandler(IStudentRepository studentRepository, IEmailService emailService) : IRequestHandler<RequestPasswordResetCommand>
{
    public async Task Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await studentRepository.GetStudentByEmail(request.Email, cancellationToken);
        }
        catch (EmailNotFoundException)
        {
            throw new UserNotFoundException($"Student with email {request.Email} doesn't exist");
        }

        await emailService.SendPasswordResetEmail(request.Email);
    }
}
