using MediatR;
using PoZiomkaDomain.Application.Dtos;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;

namespace PoZiomkaDomain.Application.Queries.GetStudent;
public class GetStudentQueryHandler(IApplicationRepository applicationRepository) :
    IRequestHandler<GetStudentQuery, IEnumerable<ApplicationDisplay>>
{
    public async Task<IEnumerable<ApplicationDisplay>> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");
        return await applicationRepository.GetAllStudentsApplications(loggedInUserId);
    }
}


