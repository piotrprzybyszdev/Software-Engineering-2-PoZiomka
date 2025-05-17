using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Common.Exceptions;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.Student;
using PoZiomkaDomain.Student.Dtos;
using PoZiomkaDomain.Student.Exceptions;
using PoZiomkaDomain.Student.Queries.GetStudent;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.Student;

public class GetStudentQueryHandlerTest
{
    private readonly Mock<IStudentRepository> mockStudentRepository;
    private readonly Mock<IMatchRepository> mockMatchRepository;
    private readonly GetStudentQueryHandler handler;

    private readonly StudentModel model;
    public GetStudentQueryHandlerTest()
    {
        mockStudentRepository = new Mock<IStudentRepository>();
        mockMatchRepository = new Mock<IMatchRepository>();
        handler = new GetStudentQueryHandler(mockStudentRepository.Object, mockMatchRepository.Object);
        model = new StudentModel(
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
            RoomId: 1,
            IsPhoneNumberHidden: true,
            IsIndexNumberHidden: false
        );
    }
    [Fact]
    public async Task AdminGetAccess()
    {
        mockStudentRepository.Setup(x =>
            x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);
        GetStudentQuery query = new(1, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
                new(ClaimTypes.Role, Roles.Administrator),
                new(ClaimTypes.NameIdentifier, "99") })));

        await handler.Handle(query, new CancellationToken());

        // check if does not throw exceptions
        Assert.True(true);
    }
    [Fact]
    public async Task UserItSelfGetAccess()
    {
        mockStudentRepository.Setup(x =>
            x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);
        GetStudentQuery query = new(1, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "1") })));

        await handler.Handle(query, new CancellationToken());

        // check if does not throw exceptions
        Assert.True(true);
    }
    [Fact]
    public async Task UserWithMatchGetAccess()
    {
        mockStudentRepository.Setup(x =>
            x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);
        mockMatchRepository.Setup(x => x.IsMatch(2, 1)).ReturnsAsync(true);
        mockMatchRepository.Setup(x => x.IsMatch(1, 2)).ReturnsAsync(true);

        GetStudentQuery query = new(2, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "1") })));

        await handler.Handle(query, new CancellationToken());

        // check if does not throw exceptions
        Assert.True(true);
    }
    [Fact]
    public async Task UserWithoutMatchDontGetAccess()
    {
        mockStudentRepository.Setup(x =>
            x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);
        mockMatchRepository.Setup(x => x.IsMatch(2, 1)).ReturnsAsync(false);
        mockMatchRepository.Setup(x => x.IsMatch(1, 2)).ReturnsAsync(false);

        GetStudentQuery query = new(2, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "1") })));

        await Assert.ThrowsAsync<UnauthorizedException>(async () =>
                    await handler.Handle(query, new CancellationToken()));
    }
    [Fact]
    public async Task AdminGetsHiddenInfo()
    {
        mockStudentRepository.Setup(x =>
            x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);
        GetStudentQuery query = new(1, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
                new(ClaimTypes.Role, Roles.Administrator),
                new(ClaimTypes.NameIdentifier, "99") })));

        var result = await handler.Handle(query, new CancellationToken());

        Assert.True(model.IsPhoneNumberHidden);
        Assert.Equal(result.PhoneNumber, model.PhoneNumber);
    }
    [Fact]
    public async Task UserGetsItSelfHiddenInfo()
    {
        mockStudentRepository.Setup(x =>
            x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);
        GetStudentQuery query = new(1, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "1") })));

        var result = await handler.Handle(query, new CancellationToken());

        Assert.True(model.IsPhoneNumberHidden);
        Assert.Equal(result.PhoneNumber, model.PhoneNumber);
    }
    [Fact]
    public async Task UserWithMatchDontGetsHiddentInfo()
    {
        mockStudentRepository.Setup(x =>
            x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync(model);
        mockMatchRepository.Setup(x => x.IsMatch(2, 1)).ReturnsAsync(true);
        mockMatchRepository.Setup(x => x.IsMatch(1, 2)).ReturnsAsync(true);

        GetStudentQuery query = new(2, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
                new(ClaimTypes.Role, Roles.Student),
                new(ClaimTypes.NameIdentifier, "1") })));

        var result = await handler.Handle(query, new CancellationToken());

        Assert.True(model.IsPhoneNumberHidden);
        Assert.Null(result.PhoneNumber);
    }

    [Fact]
    public async Task ThrowExceptionIfUserIdNotExists()
    {
        mockStudentRepository.Setup(x =>
            x.GetStudentById(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .Throws(new StudentNotFoundException("Student not found"));
        GetStudentQuery query = new(1, new ClaimsPrincipal(
                        new ClaimsIdentity(new Claim[]
                        {
        new(ClaimTypes.Role, Roles.Administrator),
        new(ClaimTypes.NameIdentifier, "99") })));

        await Assert.ThrowsAsync<StudentNotFoundException>(async () =>
                    await handler.Handle(query, new CancellationToken()));
    }
}