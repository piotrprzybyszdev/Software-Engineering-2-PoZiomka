using PoZiomkaDomain.Student;
using PoZiomkaDomain.Room;

namespace PoZiomkaApi.Services
{
	public interface IRoomSelector
	{
		public void AddStudentToRoom(Room room, Student student);
		public void DeleteStudentFromRoom(Room room, Student student);
	}
}
