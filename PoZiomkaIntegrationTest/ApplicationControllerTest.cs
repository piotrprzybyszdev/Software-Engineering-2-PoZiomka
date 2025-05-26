using PoZiomkaDomain.Admin.Dtos;
using PoZiomkaDomain.Application.Dtos;
using System.Net.Http.Json;

namespace PoZiomkaIntegrationTest;

public class ApplicationControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _userEmail = "student@example.com";
    private readonly string _userPassword = "asdf";
    private readonly string _adminEmail = "admin@example.com";
    private readonly string _adminPassword = "asdf";


    [Fact]
    public async Task GetTypes_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/application/get-types");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<IEnumerable<ApplicationTypeModel>>();
        Assert.NotNull(getResponseContent);
    }

    [Fact]
    public async Task GetApplications_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/application/get?StudentEmail=student@example.com");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<object>();
        Assert.NotNull(getResponseContent);
    }

    [Fact]
    public async Task GetStudent_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsUser(_userEmail, _userPassword);
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/application/get-student");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<object>();
        Assert.NotNull(getResponseContent);
    }
}
