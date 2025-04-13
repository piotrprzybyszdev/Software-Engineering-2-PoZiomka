using MediatR;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Application.Queries.GetTypes;
public class GetTypesQueryHandler(IApplicationRepository applicationRepository) :
    IRequestHandler<GetTypesQuery, IEnumerable<ApplicationTypeModel>>
{
    public async Task<IEnumerable<ApplicationTypeModel>> Handle(GetTypesQuery request, CancellationToken cancellationToken)
    {
        return await applicationRepository.GetTypes();
    }
}


