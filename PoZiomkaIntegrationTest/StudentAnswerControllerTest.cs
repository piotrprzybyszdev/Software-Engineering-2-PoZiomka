using PoZiomkaDomain.Form.Dtos;
using PoZiomkaDomain.StudentAnswers.Dtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PoZiomkaIntegrationTest;

public class StudentAnswerControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _userEmail = "student@example.com";
    private readonly string _userPassword = "asdf";
    private readonly int _studentId = 1;

    [Fact]
    public async Task GetStudentAnswers_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsUser(_userEmail, _userPassword);

        var request = new HttpRequestMessage(HttpMethod.Get, $"/answer/get/{_studentId}");
        var response = await _client.SendAsyncWithCookie(request, cookie);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var answers = await response.Content.ReadFromJsonAsync<IEnumerable<StudentAnswerStatus>>();
        Assert.NotNull(answers);
    }

    [Fact]
    public async Task GetSpecificStudentAnswer_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsUser(_userEmail, _userPassword);

        // Assume the student has at least one answer status
        var statusRequest = new HttpRequestMessage(HttpMethod.Get, $"/answer/get/{_studentId}");
        var statusResponse = await _client.SendAsyncWithCookie(statusRequest, cookie);
        var statusList = await statusResponse.Content.ReadFromJsonAsync<IEnumerable<StudentAnswerStatus>>();
        var formId = statusList.First().Form.Id;

        var request = new HttpRequestMessage(HttpMethod.Get, $"/answer/get/{formId}/{_studentId}");
        var response = await _client.SendAsyncWithCookie(request, cookie);
        response.EnsureSuccessStatusCode();

        var answer = await response.Content.ReadFromJsonAsync<StudentAnswerDisplay>();
        Assert.NotNull(answer);
    }

   
    [Fact]
    public async Task DeleteStudentAnswer_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsUser(_userEmail, _userPassword);

        var statusRequest = new HttpRequestMessage(HttpMethod.Get, $"/answer/get/{_studentId}");
        var statusResponse = await _client.SendAsyncWithCookie(statusRequest, cookie);
        var statuses = await statusResponse.Content.ReadFromJsonAsync<IEnumerable<StudentAnswerStatus>>();
        int answerId = statuses.First(a => a.Id.HasValue).Id.Value;

        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, $"/answer/delete/{answerId}");
        var deleteResponse = await _client.SendAsyncWithCookie(deleteRequest, cookie);
        deleteResponse.EnsureSuccessStatusCode();
    }
}
