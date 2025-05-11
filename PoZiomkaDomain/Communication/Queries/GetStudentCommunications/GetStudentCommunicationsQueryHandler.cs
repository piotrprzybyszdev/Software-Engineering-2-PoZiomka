using MediatR;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Communication.Dtos;

namespace PoZiomkaDomain.Communication.Queries.GetStudentCommunications;

public class GetStudentCommunicationsQueryHandler(ICommunicationRepository communicationRepository) : IRequestHandler<GetStudentCommunicationsQuery, IEnumerable<CommunicationDisplay>>
{
    public async Task<IEnumerable<CommunicationDisplay>> Handle(GetStudentCommunicationsQuery request, CancellationToken cancellationToken)
    {
        int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        return await communicationRepository.GetStudentCommunications(loggedInUserId, cancellationToken);
    }
}
