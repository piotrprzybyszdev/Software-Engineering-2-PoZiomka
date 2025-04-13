using Moq;
using PoZiomkaDomain.Application;
using PoZiomkaDomain.Application.Queries.GetStudent;
using PoZiomkaDomain.Common.Exceptions;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.Application;

public class GetStudentQueryHandlerTest
{
    [Fact]
    public async Task OnInvalidUserId_ThrowsException()
    {
        var getStudentQuery = new GetStudentQuery(new ClaimsPrincipal(new ClaimsIdentity()));

        var applicationRepository = new Mock<IApplicationRepository>();

        var handler = new GetStudentQueryHandler(applicationRepository.Object);

        var exception = await Assert.ThrowsAsync<DomainException>(() => handler.Handle(getStudentQuery, default));

        Assert.Equal("Id of the user isn't known", exception.Message);
    }

    [Fact]
    public async Task GetsStudent()
    {
        var getStudentQuery = new GetStudentQuery(new ClaimsPrincipal(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));

        var applicationRepository = new Mock<IApplicationRepository>();

        var handler = new GetStudentQueryHandler(applicationRepository.Object);

        await handler.Handle(getStudentQuery, default);

        applicationRepository.Verify(x => x.GetAllStudentsApplications(1));
    }
}
