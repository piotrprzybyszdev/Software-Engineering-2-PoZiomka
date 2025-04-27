using Moq;
using PoZiomkaDomain.Common;
using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.Match;
using PoZiomkaDomain.StudentAnswers;
using PoZiomkaDomain.StudentAnswers.Dtos;
using PoZiomkaDomain.StudentAnswers.Queries.GetAnswer;
using System.Security.Claims;

namespace PoZiomkaUnitTest.Domain.StudentAnswers;
public class GetAnswerQueryHandlerTest
{
    [Fact]
    public async Task StudentNotMatchThrowException()
    {
        var studentId = 1;
        var queryStudentId = 2;

        // Arrange
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Role, Roles.Student),
                new Claim(ClaimTypes.NameIdentifier, studentId.ToString())
            ]));

        var mockStudentAnswerRepository = new Mock<IStudentAnswerRepository>();
        var mockJudgeService = new Mock<IJudgeService>();

        mockJudgeService.Setup(x => x.IsMatch(2, 1)).ReturnsAsync(false);
        mockJudgeService.Setup(x => x.IsMatch(1, 2)).ReturnsAsync(false);

        mockStudentAnswerRepository
            .Setup(repo => repo.GetStudentAnswer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentAnswerDisplay(queryStudentId, 1, 1, FormStatus.InProgress, [], []));

        var query = new GetAnswerQuery(user, formId: 1, queryStudentId);
        var handler = new GetAnswerQueryHandler(mockStudentAnswerRepository.Object, mockJudgeService.Object);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await handler.Handle(query, new CancellationToken()));
    }

    [Fact]
    public async Task StudentItselfSeeHiddenAnswers()
    {
        var studentId = 1;
        var queryStudentId = 1;

        // Arrange
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Role, Roles.Student),
                new Claim(ClaimTypes.NameIdentifier, studentId.ToString())
            ])
        );

        StudentAnswerChoosableDisplay choosableAnswer = new(1, "as", true);
        ObligatoryPreferenceDisplay obligatoryAnswera = new(1, "a", []);

        StudentAnswerObligatoryDisplay obligatoryAnswer = new(1, obligatoryAnswera, 1, true);

        var mockStudentAnswerRepository = new Mock<IStudentAnswerRepository>();
        var mockJudgeService = new Mock<IJudgeService>();

        mockJudgeService.Setup(x => x.IsMatch(2, 1)).ReturnsAsync(true);
        mockJudgeService.Setup(x => x.IsMatch(1, 2)).ReturnsAsync(true);

        mockStudentAnswerRepository
            .Setup(repo => repo.GetStudentAnswer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentAnswerDisplay(queryStudentId, 1, 1, FormStatus.InProgress,
            [choosableAnswer], [obligatoryAnswer]));

        var query = new GetAnswerQuery(user, formId: 1, queryStudentId);
        var handler = new GetAnswerQueryHandler(mockStudentAnswerRepository.Object, mockJudgeService.Object);

        var result = await handler.Handle(query, new CancellationToken());

        Assert.NotEmpty(result.ChoosableAnswers);
        Assert.NotEmpty(result.ObligatoryAnswers);
    }

    [Fact]
    public async Task MatchStudentDoNotSeeHiddenAnswers()
    {
        var studentId = 1;
        var queryStudentId = 2;

        // Arrange
        var user = new ClaimsPrincipal(
            new ClaimsIdentity(
            [
                new Claim(ClaimTypes.Role, Roles.Student),
                new Claim(ClaimTypes.NameIdentifier, studentId.ToString())
            ]));

        StudentAnswerChoosableDisplay choosableAnswer = new(1, "as", true);
        ObligatoryPreferenceDisplay obligatoryAnswera = new(1, "a", []);

        StudentAnswerObligatoryDisplay obligatoryAnswer = new(1, obligatoryAnswera, 1, true);

        var mockStudentAnswerRepository = new Mock<IStudentAnswerRepository>();
        var mockJudgeService = new Mock<IJudgeService>();

        mockJudgeService.Setup(x => x.IsMatch(2, 1)).ReturnsAsync(true);
        mockJudgeService.Setup(x => x.IsMatch(1, 2)).ReturnsAsync(true);

        mockStudentAnswerRepository
            .Setup(repo => repo.GetStudentAnswer(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StudentAnswerDisplay(queryStudentId, 1, 1, FormStatus.InProgress,
            [choosableAnswer], [obligatoryAnswer]));

        var query = new GetAnswerQuery(user, formId: 1, queryStudentId);
        var handler = new GetAnswerQueryHandler(mockStudentAnswerRepository.Object, mockJudgeService.Object);

        var result = await handler.Handle(query, new CancellationToken());

        Assert.Empty(result.ChoosableAnswers);
        Assert.Empty(result.ObligatoryAnswers);
    }
}

