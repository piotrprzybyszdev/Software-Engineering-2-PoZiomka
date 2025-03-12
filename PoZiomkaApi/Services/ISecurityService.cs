using PoZiomkaDomain.Student;

namespace PoZiomkaApi.Services
{
	public interface ISecurityService
	{
		public Student GetLoggedStudent();
		public void RegisterStudent(Student student);
		public void LogStudent(Student student);
	}
}
