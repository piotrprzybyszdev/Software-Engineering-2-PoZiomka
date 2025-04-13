using MediatR;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Room.Dtos;
using PoZiomkaDomain.Student;

namespace PoZiomkaDomain.Room.Queries.GetRoom;

public class GetRoomQueryHandler(IRoomRepository roomRepository, IStudentRepository studentRepository) : IRequestHandler<GetRoomQuery, RoomStudentDisplay>
{
    public async Task<RoomStudentDisplay> Handle(GetRoomQuery request, CancellationToken cancellationToken)
    {
        int loggedInUserId = request.User.GetUserId() ?? throw new DomainException("Id of the user isn't known");

        bool isUserAuthorized = false;

        if (request.User.IsInRole(Roles.Administrator)) isUserAuthorized = true;
        else if (request.User.IsInRole(Roles.Student))
        {
            var student = await studentRepository.GetStudentById(loggedInUserId, cancellationToken);
            if (student == null)
                throw new UserNotFoundException($"Student with id `{loggedInUserId}` not found");
            if (student.RoomId == request.Id)
                isUserAuthorized = true;
        }

        if (!isUserAuthorized)
            throw new UnauthorizedException("User must be logged in as an administrator or a student that is assigned to the room");

        RoomModel room;
        try
        {
            room = await roomRepository.GetRoomById(request.Id, cancellationToken);
        }
        catch
        {
            throw new RoomNotFoundException($"Room with id `{request.Id}` not found");
        }

        var students = await studentRepository.GetStudentsByRoomId(request.Id, cancellationToken);

        var hidePersonalInfo = !(request.User.IsInRole(Roles.Administrator));

        var roomStudentDisplay = new RoomStudentDisplay(room.Id, room.Floor, room.Number, room.Capacity, null, students.Select(x => x.ToStudentDisplay(hidePersonalInfo)));

        return roomStudentDisplay;
    }
}
