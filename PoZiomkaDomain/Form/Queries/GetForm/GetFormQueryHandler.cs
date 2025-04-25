using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.Room;

namespace PoZiomkaDomain.Form.Commands.GetForm;

public class GetFormQueryHandler(IFormRepository formRepository) : IRequestHandler<GetFormQuery, FormDisplay>
{
    public async Task<FormDisplay> Handle(GetFormQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var form = await formRepository.GetFormDisplay(request.Id, cancellationToken);
            return form;
        }
        catch (IdNotFoundException e)
        {
            throw new DomainException($"Form with id `{request.Id}` not found", e);
        }
    }
}
