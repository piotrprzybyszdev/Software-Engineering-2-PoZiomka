using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Student.Exceptions;

namespace PoZiomkaDomain.Communication.Commands.SendCommunication;

public class SendCommunicationCommandHandler(ICommunicationRepository communicationRepository) : IRequestHandler<SendCommunicationCommand>
{
    public async Task Handle(SendCommunicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await communicationRepository.CreateCommunications(request.StudentIds, request.Description, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new StudentNotFoundException("Student not found", e);
        }
    }
}
