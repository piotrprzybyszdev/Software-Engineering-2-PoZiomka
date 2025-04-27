using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.Form.Exceptions;

namespace PoZiomkaDomain.Form.Commands.UpdateForm;

public class UpdateFormCommandHandler(IFormRepository formRepository) : IRequestHandler<UpdateFormCommand>
{
    public async Task Handle(UpdateFormCommand request, CancellationToken cancellationToken)
    {
        var formUpdate = new FormUpdate(request.Id, request.Title, request.ObligatoryPreferences);
        try
        {
            await formRepository.UpdateForm(formUpdate, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new FormNotFoundException("Form not found", e);
        }
    }
}
