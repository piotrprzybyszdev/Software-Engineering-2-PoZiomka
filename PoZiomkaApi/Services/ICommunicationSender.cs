using PoZiomkaDomain.Communication;
using PoZiomkaDomain.Student;

namespace PoZiomkaApi.Services
{
	public interface ICommunicationSender
	{
		public void SendCommunication(Communication communication, List<Student> students);
	}
}
