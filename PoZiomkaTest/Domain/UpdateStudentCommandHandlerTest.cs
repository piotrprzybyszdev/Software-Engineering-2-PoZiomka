using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.SignupStudent;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaTest.Domain;

public class UpdateStudentCommandHandlerTest
{
	[Fact]
	public async Task AdminGetAccess()
	{
		Assert.True(false);
	}
	[Fact]
	public async Task UserItSelfGetAccess()
	{
		Assert.True(false);
	}
	[Fact]
	public async Task UserCanNotEditAnotherUser()
	{
		Assert.True(false);
	}
	[Fact]
	public async Task ThrowExceptionIfUserIdNotExists()
	{
		Assert.True(false);
	}
}
