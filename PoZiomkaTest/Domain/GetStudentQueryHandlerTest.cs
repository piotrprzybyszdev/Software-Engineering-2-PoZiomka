using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Exceptions;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Commands.SignupStudent;
using PoZiomkaDomain.Student.Dtos;

namespace PoZiomkaTest.Domain;

public class GetStudentQueryHandlerTest
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
	public async Task UserWithMatchGetAccess()
	{
		Assert.True(false);
	}
	[Fact]
	public async Task UserWithoutMatchDontGetAccess()
	{
		Assert.True(false);
	}
	[Fact]
	public async Task AdminGetsHiddenInfo()
	{
		Assert.True(false);
	}
	[Fact]
	public async Task UserGetsItSelfHiddenInfo()
	{
		Assert.True(false);
	}
	[Fact]
	public async Task UserWithMatchDontGetsHiddentInfo()
	{
		Assert.True(false);
	}

	[Fact]
	public async Task ThrowExceptionIfUserIdNotExists()
	{
		Assert.True(false);
	}
}