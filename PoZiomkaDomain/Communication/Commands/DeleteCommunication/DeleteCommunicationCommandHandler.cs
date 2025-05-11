using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Communication.Exceptions;

namespace PoZiomkaDomain.Communication.Commands.DeleteCommunication;

public class DeleteCommunicationCommandHandler(ICommunicationRepository communicationRepository) : IRequestHandler<DeleteCommunicationCommand>
{
    public async Task Handle(DeleteCommunicationCommand request, CancellationToken cancellationToken)
    {
        int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        try
        {
            if (loggedInUserId != await communicationRepository.GetStudentIdByCommunicationId(request.Id, cancellationToken))
                throw new UnauthorizedDeleteException("User must be logged in as the student to delete");
            await communicationRepository.DeleteCommunication(request.Id, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new CommunicationNotFoundException("Communication not found", e);
        }
    }
}