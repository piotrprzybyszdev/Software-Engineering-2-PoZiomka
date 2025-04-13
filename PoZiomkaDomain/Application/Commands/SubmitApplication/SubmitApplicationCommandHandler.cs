using MediatR;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Application.Exceptions;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Commands.SubmitApplication;

public class SubmitApplicationCommandHandler(IApplicationRepository applicationRepository, IFileStorage fileStorage) : IRequestHandler<SubmitApplicationCommand>
{
    public async Task Handle(SubmitApplicationCommand request, CancellationToken cancellationToken)
    {
        int loggedUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        Guid guid = Guid.NewGuid();

        try
        {
            await fileStorage.UploadFile(guid, request.File);
        }
        catch (FileTooLargeException e)
        {
            throw new InvalidFileException($"File of size `{e.Size}` is too large max size is `{e.MaxSize}`");
        }

        try
        {
            await applicationRepository.Submit(loggedUserId, request.Id, guid, ApplicationStatus.Pending);
        }
        catch (IdNotFoundException)
        {
            throw new ApplicationTypeNotFoundException($"Appliation type with id `{request.Id}` not found");
        }
    }
}
