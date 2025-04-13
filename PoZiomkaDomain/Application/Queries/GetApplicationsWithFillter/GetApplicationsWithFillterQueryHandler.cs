
using MediatR;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Exceptions;

namespace PoZiomkaDomain.Application.Queries.GetApplicationsWithFillter;
public class GetApplicationsWithFillterQueryHandler(IApplicationRepository applicationRepository) :
    IRequestHandler<GetApplicationsWithFillterQuery, IEnumerable<ApplicationDisplay>>
{
    public async Task<IEnumerable<ApplicationDisplay>> Handle(GetApplicationsWithFillterQuery request, CancellationToken cancellationToken)
    {
        return await applicationRepository.GetAll(
                request.StudentEmail, request.StudentIndex,
                request.ApplicationTypeId, request.ApplicationStatus);
    }
}