using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PoZiomkaDomain.Student;

namespace PoZiomkaDomain.Room
{
	public class Room
	{
		private int Id;
		public int floor;
		private int number;
		public int capacity;
		private List<Student.Student> Residents;
	}
}
