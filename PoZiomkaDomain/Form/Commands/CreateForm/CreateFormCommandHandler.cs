using MediatR;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form.Commands.CreateForm;

public class CreateFormCommandHandler(IFormRepository formRepository) : IRequestHandler<CreateFormCommand>
{
    public async Task Handle(CreateFormCommand request, CancellationToken cancellationToken)
    {
        var formCreate = new FormCreate(request.Title, request.ObligatoryPreferences);
        await formRepository.CreateForm(formCreate, cancellationToken);
    }
}
