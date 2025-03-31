using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.DeleteStudent;
using PoZiomkaDomain.Student.Commands.SignupStudent;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaTest.Domain;

public class DeleteStudentCommandHandlerTest
{
	[Fact]
	public async Task ThrowExceptionIfUserNotExists()
	{
		DeleteStudentCommand command = new(1);

		var studentRepository = new Mock<IStudentRepository>();
		studentRepository.Setup(x => x.DeleteStudent(It.IsAny<int>(),
			It.IsAny<CancellationToken>()))
			.ThrowsAsync(new NoRowsEditedException());

		DeleteStudentCommandHandler handler = new(studentRepository.Object);

		await Assert.ThrowsAsync<StudentNotFoundException>(
			() => handler.Handle(command, default)
		);
	}

}
