using PoZiomkaDomain.Admin.Dtos;
using PoZiomkaDomain.Form.Dtos;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PoZiomkaIntegrationTest;

public class AdminControllerTest(MockWebApplicationFactory<Program> _factory) : IClassFixture<MockWebApplicationFactory<Program>>
{
    private readonly HttpClient _client = _factory.CreateClient();

    private readonly string _adminEmail = "admin@example.com";
    private readonly string _adminPassword = "asdf";

    [Fact]
    public async Task GetLoggedInAdmin_ShouldReturnSuccess()
    {
        string cookie = await _client.LoginAsAdmin(_adminEmail, _adminPassword);
        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/admin/get-logged-in");
        var response = await _client.SendAsyncWithCookie(getRequest, cookie);
        response.EnsureSuccessStatusCode();
        var getResponseContent = await response.Content.ReadFromJsonAsync<AdminDisplay>();
        Assert.NotNull(getResponseContent);
        Assert.Equal(_adminEmail, getResponseContent.Email);
    }
}
