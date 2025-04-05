using Moq;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.CreateStudent;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaTest.Domain;

public class CreateStudentCommandHandlerTest
{
    [Fact]
    public async Task CreatesStudent()
    {
        var command = new CreateStudentCommand("test@gmail.com", "John", "Doe", "S123456", "123456789");

        var studentRepository = new Mock<IStudentRepository>();

        var handler = new CreateStudentCommandHandler(studentRepository.Object);
        await handler.Handle(command, default);

        studentRepository.Verify(x => x.CreateStudent(It.IsAny<StudentCreate>(), It.IsAny<CancellationToken?>()), Times.Once);
    }

    [Fact]
    public async Task ThrowsEmailNotUniqueException()
    {
        var command = new CreateStudentCommand("test@gmail.com", "John", "Doe", "S123456", "123456789");

        var studentRepository = new Mock<IStudentRepository>();
        studentRepository.Setup(x => x.CreateStudent(It.IsAny<StudentCreate>(), It.IsAny<CancellationToken?>()))
            .ThrowsAsync(new EmailNotUniqueException());

        var handler = new CreateStudentCommandHandler(studentRepository.Object);

        await Assert.ThrowsAsync<EmailTakenException>(() => handler.Handle(command, default));
    }
}
