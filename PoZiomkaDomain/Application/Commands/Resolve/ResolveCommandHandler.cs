using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Application.Commands.Resolve;
public class ResolveCommandHandler(IApplicationRepository applicationRepository) : IRequestHandler<ResolveCommand>
{
    public async Task Handle(ResolveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await applicationRepository.Update(request.Id, request.Status);
        }
        catch(IdNotFoundException ex)
        {
            throw new ApplicationNotFoundException(ex.Message);
        }
    }
}


