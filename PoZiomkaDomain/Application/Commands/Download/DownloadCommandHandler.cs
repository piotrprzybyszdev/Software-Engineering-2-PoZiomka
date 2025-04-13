using MediatR;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Application.Commands.Download;
public class DownloadCommandHandler(IApplicationRepository applicationRepository, IFileStorage fileStorage) : IRequestHandler<DownloadCommand, IFile>
{
    public async Task<IFile> Handle(DownloadCommand request, CancellationToken cancellationToken)
    {
        int loggedUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        ApplicationModel application;
        try
        {
            application = await applicationRepository.Get(request.Id);
            if (application == null)
            {
                throw new DomainException("Application not found");
            }
        }
        catch
        {
            throw new DomainException("Error in downloading file");
        }

        if (loggedUserId != application.StudentId)
        {
            throw new DomainException("You are not allowed to download this file");
        }


        IFile file;
        try
        {
            file = await fileStorage.GetFileByGuid(application.FileGuid);
        }
        catch
        {
            throw new DomainException("Error in downloading file");
        }

        return file;
    }
}