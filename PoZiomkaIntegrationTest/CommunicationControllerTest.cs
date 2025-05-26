using PoZiomkaDomain.Communication.Dtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PoZiomkaIntegrationTest;

public class CommunicationControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _userEmail = "student@example.com";
    private readonly string _userPassword = "asdf";
    private readonly string _adminEmail = "admin@example.com";
    private readonly string _adminPassword = "asdf";

    [Fact]
    public async Task GetMessages_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsUser(_userEmail, _userPassword);
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/communication/get-student");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<IEnumerable<CommunicationDisplay>>();
        Assert.NotNull(getResponseContent);
    }

    [Fact]
    public async Task SendMessage_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);
        var postRequest = new HttpRequestMessage(HttpMethod.Post, "/communication/send");
        var message = new
        {
            StudentIds = new List<int>() { 1 },
            Description = "Message"
        };
        postRequest.Content = new StringContent(JsonSerializer.Serialize(message), Encoding.UTF8, "application/json");

        var response = await _client.SendAsyncWithCookie(postRequest, cookie);
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteMessage_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsUser(_userEmail, _userPassword);
        var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, "/communication/delete/1");
        var response = await _client.SendAsyncWithCookie(deleteRequest, cookie);
        response.EnsureSuccessStatusCode();
    }
}
