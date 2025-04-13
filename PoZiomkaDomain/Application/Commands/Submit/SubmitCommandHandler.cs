using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Application.Commands.Submit;
public class SubmitCommandHandler(IApplicationRepository applicationRepository, IFileStorage fileStorage) : IRequestHandler<SubmitCommand>
{
    public async Task Handle(SubmitCommand request, CancellationToken cancellationToken)
    {
        int loggedUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        Guid guid = Guid.NewGuid();

        await fileStorage.UploadFile(guid, request.File);
        await applicationRepository.Submit(loggedUserId, request.Id, guid, Dtos.ApplicationStatus.Pending);
    }
}
