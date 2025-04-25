using MediatR;
using PoZiomkaDomain.Form.Dtos;

namespace PoZiomkaDomain.Form.Commands.GetAllForms;

public class GetAllFormsQueryHandler(IFormRepository formRepository) : IRequestHandler<GetAllFormsQuery, IEnumerable<FormModel>>
{
    public async Task<IEnumerable<FormModel>> Handle(GetAllFormsQuery request, CancellationToken cancellationToken)
    {
        return await formRepository.GetForms(cancellationToken);
    }
}
