using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Commands.SubmitApplication;

public class SubmitApplicationCommandHandler(IApplicationRepository applicationRepository, IFileStorage fileStorage) : IRequestHandler<SubmitApplicationCommand>
{
    public async Task Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        int loggedUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        Guid guid = Guid.NewGuid();

        await fileStorage.UploadFile(guid, request.File);
        await applicationRepository.Submit(loggedUserId, request.Id, guid, Dtos.ApplicationStatus.Pending);
    }
}
