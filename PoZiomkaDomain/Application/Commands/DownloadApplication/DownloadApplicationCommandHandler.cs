using MediatR;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Application.Exceptions;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Commands.DownloadApplication;

public class DownloadApplicationCommandHandler(IApplicationRepository applicationRepository, IFileStorage fileStorage) : IRequestHandler<DownloadApplicationCommand, IFile>
{
    public async Task<IFile> Handle(DownloadApplicationCommand request, CancellationToken cancellationToken)
    {
        int loggedUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        ApplicationModel application;

        application = await applicationRepository.Get(request.Id);
        if (application == null)
        {
            throw new ApplicationNotFoundException($"Application with id `{request.Id}` not found");
        }

        if (loggedUserId != application.StudentId)
        {
            throw new ApplicationOwnershipException($"Logged in user cannot access application with id `{request.Id}`");
        }

        return await fileStorage.GetFileByGuid(application.FileGuid);
    }
}