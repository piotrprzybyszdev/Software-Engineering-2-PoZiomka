using PoZiomkaDomain.Student;
using PoZiomkaDomain.Room;

namespace PoZiomkaApi.Services
{
	public class RoomSelector(ISecurityService securityService) : IRoomSelector
	{
		public void AddStudentToRoom(Room room, Student student)
		{
		}
		public void DeleteStudentFromRoom(Room room, Student student)
		{
		}
	}
}
