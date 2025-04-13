
using MediatR;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Application.Queries.GetApplicationsWithFillter;
public class GetApplicationsWithFillterQueryHandler(IApplicationRepository applicationRepository) :
    IRequestHandler<GetApplicationsWithFillterQuery, IEnumerable<ApplicationModel>>
{
    public async Task<IEnumerable<ApplicationModel>> Handle(GetApplicationsWithFillterQuery request, CancellationToken cancellationToken)
    {
        return await applicationRepository.GetAll(
                request.StudentEmail, request.StudentIndex,
                request.ApplicationTypeId, request.ApplicationStatus);
    }
}