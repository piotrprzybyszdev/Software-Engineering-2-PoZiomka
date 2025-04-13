using PoZiomkaDomain.Room.Commands.AddStudentToRoom;

namespace PoZiomkaApi.Requests.Room;

public record AddStudentRequest(int Id, int StudentId)
{
    public AddStudentCommand ToAddStudentCommand() => new(Id, StudentId);
}
