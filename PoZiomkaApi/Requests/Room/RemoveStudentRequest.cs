using PoZiomkaDomain.Room.Commands.RemoveStudent;

namespace PoZiomkaApi.Requests.Room;

public record RemoveStudentRequest(int Id, int StudentId)
{
    public RemoveStudentCommand ToRemoveStudentCommand() => new(Id, StudentId);
}
