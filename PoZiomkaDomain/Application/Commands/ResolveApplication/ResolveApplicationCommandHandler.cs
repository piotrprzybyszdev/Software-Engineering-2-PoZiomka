using MediatR;
using PoZiomkaDomain.Application.Exceptions;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Commands.ResolveApplication;

public class ResolveApplicationCommandHandler(IApplicationRepository applicationRepository) : IRequestHandler<ResolveApplicationCommand>
{
    public async Task Handle(ResolveApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await applicationRepository.Update(request.Id, request.Status);
        }
        catch (IdNotFoundException)
        {
            throw new ApplicationNotFoundException($"Application with id `{request.Id}` doesn't exist");
        }
    }
}


