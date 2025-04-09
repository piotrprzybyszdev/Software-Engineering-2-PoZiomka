using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student;
using System.Threading;

namespace PoZiomkaDomain.Room.Queries.GetRoom;

public class GetRoomQueryHandler(IRoomRepository roomRepository): IRequestHandler<GetRoomQuery, RoomDisplay>
{
    public async Task<RoomDisplay> Handle(GetRoomQuery request, CancellationToken cancellationToken)
    {
        int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");
        int userId = request.Id ?? loggedInUserId;

        bool isUserAuthorized = request.User.IsInRole(Roles.Administrator) ||
          request.User.IsInRole(Roles.Student) &&
          (loggedInUserId == userId || await judgeService.IsMatch(loggedInUserId, userId));

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or a student that has a match with the student");

        var hidePersonalInfo = !(request.User.IsInRole(Roles.Administrator) || loggedInUserId == userId);

        try
        {
            var student = await roomRepository.GetRoomById(roomId, cancellationToken);
            return student.ToStudentDisplay(hidePersonalInfo);
        }
        catch
        {
            throw new UserNotFoundException($"Student with id `{userId}` not found");
        }
    }
}
{
    
}
