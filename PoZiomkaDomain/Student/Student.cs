using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoZiomkaDomain.Student
{
	public class Student(int Id, string Email, string Phone)
	{
		private int Id = Id;
		private string Email = Email;
		private string Phone = Phone;
		private StudentAnswers StudentAnswers = new StudentAnswers();
	}
}
