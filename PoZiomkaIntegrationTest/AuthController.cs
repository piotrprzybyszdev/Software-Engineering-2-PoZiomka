using System.Net;
using System.Text;
using System.Text.Json;

namespace PoZiomkaIntegrationTest;

public class AuthController(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _userEmail = "student@example.com";
    private readonly string _userPassword = "asdf";
    private readonly string _adminEmail = "admin@example.com";
    private readonly string _adminPassword = "asdf";

    [Fact]
    public async Task LoginAsUser_ShouldReturnSuccess()
    {
        var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/login");
        var loginData = new
        {
            Email = _userEmail,
            Password = _userPassword
        };
        loginRequest.Content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(loginRequest);
        response.EnsureSuccessStatusCode();
        var cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        Assert.NotNull(cookie);
    }

    [Fact]
    public async Task LoginAsAdmin_ShouldReturnSuccess()
    {
        var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/admin-login");
        var loginData = new
        {
            Email = _adminEmail,
            Password = _adminPassword
        };
        loginRequest.Content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(loginRequest);
        response.EnsureSuccessStatusCode();
        var cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        Assert.NotNull(cookie);
    }

    [Fact]
    public async Task Logout_ShouldReturnSuccess()
    {
        var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/login");
        var loginData = new
        {
            Email = _userEmail,
            Password = _userPassword
        };
        loginRequest.Content = new StringContent(JsonSerializer.Serialize(loginData), Encoding.UTF8, "application/json");
        var response = await _client.SendAsync(loginRequest);
        response.EnsureSuccessStatusCode();
        var cookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
        Assert.NotNull(cookie);
        var logoutRequest = new HttpRequestMessage(HttpMethod.Post, "/logout");
        logoutRequest.Headers.Add("Cookie", cookie);
        var logoutResponse = await _client.SendAsync(logoutRequest);
        logoutResponse.EnsureSuccessStatusCode();
    }
}
