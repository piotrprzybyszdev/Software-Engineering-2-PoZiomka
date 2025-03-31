using Azure.Core;
using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.SignupStudent;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student.Queries.GetAllStudents;

namespace PoZiomkaTest.Domain;

public class GetAllStudnetQueryHandlerTest
{
    [Fact]
    public async Task CheckConverting()
    {
		var student = new StudentModel(
			Id: 1,
			Email: "test@student.com",
			FirstName: "John",
			LastName: "Doe",
			PasswordHash: "hashedpassword123",
			IsConfirmed: true,
			PhoneNumber: "123-456-7890",
			IndexNumber: "S123456",
			ReservationId: 1001,
			HasAcceptedReservation: true,
			IsPhoneNumberHidden: false,
			IsIndexNumberHidden: false
		);

		var studentRepository = new Mock<IStudentRepository>();
		studentRepository.Setup(x => x.GetAllStudents(It.IsAny<CancellationToken>()))
            .ReturnsAsync([student]);

		GetAllStudentsQuery getAllStudentsQuery = new();

		GetAllStudentsCommandHandler handler = new(studentRepository.Object);
		var result = await handler.Handle(getAllStudentsQuery, default);

		Assert.Equal(result.First(), student.ToStudentDisplay(false));
	}
}
