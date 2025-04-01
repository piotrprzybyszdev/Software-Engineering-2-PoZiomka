using Moq;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.DeleteStudent;

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
			.ThrowsAsync(new IdNotFoundException());

		DeleteStudentCommandHandler handler = new(studentRepository.Object);

		await Assert.ThrowsAsync<UserNotFoundException>(
			() => handler.Handle(command, default)
		);
	}
}
