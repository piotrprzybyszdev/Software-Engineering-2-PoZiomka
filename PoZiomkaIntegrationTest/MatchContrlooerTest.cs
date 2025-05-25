using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.Match.Dtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PoZiomkaIntegrationTest;

public class MatchControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _userEmail = "student@example.com";
    private readonly string _userPassword = "asdf";

    [Fact]
    public async Task GetStudentMatches_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsUser(_userEmail, _userPassword);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/match/get-student");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();

        var matches = await response.Content.ReadFromJsonAsync<IEnumerable<MatchModel>>();
        Assert.NotNull(matches);
        Assert.NotEmpty(matches);
    }

    [Fact]
    public async Task UpdateMatch_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsUser(_userEmail, _userPassword);

        // First get existing matches
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/match/get-student");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();

        var matches = await response.Content.ReadFromJsonAsync<IEnumerable<MatchModel>>();
        Assert.NotNull(matches);
        Assert.NotEmpty(matches);

        var matchBefore = matches.First();

        var matchId = matches.First().Id;

        // Update the match (accept it)
        var update = new
        {
            Id = matchId,
            IsAcceptation = true
        };
        var updateContent = new StringContent(JsonSerializer.Serialize(update), Encoding.UTF8, "application/json");
        var putRequest = new HttpRequestMessage(HttpMethod.Put, "/match/update")
        {
            Content = updateContent
        };
        var updateResponse = await _client.SendAsyncWithCookie(putRequest, cookie);
        updateResponse.EnsureSuccessStatusCode();

        // Verify the match was updated
        var getUpdatedRequest = new HttpRequestMessage(HttpMethod.Get, "/match/get-student");
        var updatedResponse = await _client.SendAsyncWithCookie(getUpdatedRequest, cookie);
        updatedResponse.EnsureSuccessStatusCode();
        var updatedMatches = await updatedResponse.Content.ReadFromJsonAsync<IEnumerable<MatchModel>>();
        Assert.NotNull(updatedMatches);
        Assert.NotEmpty(updatedMatches);
        var matchAfter = updatedMatches.First(m => m.Id == matchId);
        Assert.Equal(matchBefore.Id, matchAfter.Id);
        Assert.True(matchAfter.Status1==MatchStatus.Accepted);
    }
}
