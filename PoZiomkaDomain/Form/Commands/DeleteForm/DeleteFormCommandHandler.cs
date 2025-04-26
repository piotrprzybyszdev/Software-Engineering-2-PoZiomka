using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Room.Exceptions;
using PoZiomkaDomain.Room;
using PoZiomkaDomain.Form.Exceptions;

namespace PoZiomkaDomain.Form.Commands.DeleteForm;

public record DeleteFormCommandHandler(IFormRepository formRepository) : IRequestHandler<DeleteFormCommand>
{
    public async Task Handle(DeleteFormCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await formRepository.DeleteForm(request.Id, cancellationToken);
        }
        catch (IdNotFoundException e)
        {
            throw new FormNotFoundException("Form not found", e);
        }
    }
}
